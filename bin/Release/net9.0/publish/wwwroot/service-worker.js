const CACHE_VERSION = 'v1.0.0';
const CACHE_NAME = `perfumeria-anita-${CACHE_VERSION}`;
const RUNTIME_CACHE = `perfumeria-anita-runtime-${CACHE_VERSION}`;

// Assets críticos que cachear en instalación
const ASSETS_TO_CACHE = [
  '/',
  '/index.html',
  '/css/app.css',
  '/css/bootstrap/bootstrap.min.css',
  '/js/app.js',
  '/manifest.json',
  '/icon-192x192.png',
  '/icon-512x512.png'
];

console.log('[Service Worker] Version:', CACHE_VERSION);

// ============================================================
// INSTALL - Cachear assets críticos
// ============================================================
self.addEventListener('install', event => {
  console.log('[Service Worker] Installing...');
  
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('[Service Worker] Caching critical assets');
        return cache.addAll(ASSETS_TO_CACHE);
      })
      .then(() => {
        console.log('[Service Worker] All assets cached');
        return self.skipWaiting(); // Activar inmediatamente
      })
      .catch(error => {
        console.error('[Service Worker] Install error:', error);
      })
  );
});

// ============================================================
// ACTIVATE - Limpiar caches viejos
// ============================================================
self.addEventListener('activate', event => {
  console.log('[Service Worker] Activating...');
  
  event.waitUntil(
    caches.keys()
      .then(cacheNames => {
        console.log('[Service Worker] Found caches:', cacheNames);
        
        return Promise.all(
          cacheNames.map(cacheName => {
            // Si la versión no coincide, eliminar
            if (!cacheName.includes(CACHE_VERSION)) {
              console.log('[Service Worker] Deleting old cache:', cacheName);
              return caches.delete(cacheName);
            }
          })
        );
      })
      .then(() => {
        console.log('[Service Worker] Cache cleanup complete');
        return self.clients.claim(); // Tomar control inmediatamente
      })
  );
});

// ============================================================
// FETCH - Estrategia de caché/red
// ============================================================
self.addEventListener('fetch', event => {
  const { request } = event;
  const url = new URL(request.url);

  // NO cachear métodos que no sean GET
  if (request.method !== 'GET') {
    return;
  }

  // ESTRATEGIA 1: API calls - Network first, fallback cache
  if (url.pathname.startsWith('/api/')) {
    event.respondWith(
      fetch(request)
        .then(response => {
          // Si OK, guardar en runtime cache y retornar
          if (response && response.status === 200) {
            const clonedResponse = response.clone();
            caches.open(RUNTIME_CACHE)
              .then(cache => {
                cache.put(request, clonedResponse);
              })
              .catch(err => console.error('[Service Worker] Cache write error:', err));
            return response;
          }
          return response;
        })
        .catch(error => {
          // Si falla red, intentar caché
          console.warn('[Service Worker] Network failed for:', url.pathname, error);
          return caches.match(request)
            .then(cached => {
              if (cached) {
                console.log('[Service Worker] Serving from cache:', url.pathname);
                return cached;
              }
              // Si no está en caché, retornar error
              return new Response(
                JSON.stringify({ error: 'Sin conexión. No hay datos en caché.' }),
                {
                  status: 503,
                  statusText: 'Service Unavailable',
                  headers: { 'Content-Type': 'application/json' }
                }
              );
            });
        })
    );
    return;
  }

  // ESTRATEGIA 2: Assets estáticos - Cache first, fallback network
  if (request.destination === 'style' ||
      request.destination === 'script' ||
      request.destination === 'image' ||
      url.pathname.endsWith('.woff2') ||
      url.pathname.endsWith('.woff') ||
      url.pathname.endsWith('.ttf')) {
    
    event.respondWith(
      caches.match(request)
        .then(cached => {
          if (cached) {
            return cached;
          }
          
          // No está en caché, buscar en red
          return fetch(request)
            .then(response => {
              // Si OK, guardar en caché y retornar
              if (response && response.status === 200) {
                const clonedResponse = response.clone();
                caches.open(RUNTIME_CACHE)
                  .then(cache => {
                    cache.put(request, clonedResponse);
                  });
                return response;
              }
              return response;
            })
            .catch(() => {
              // Sin conexión y sin caché
              if (request.destination === 'image') {
                return new Response('', { status: 404 });
              }
              return new Response('', { status: 503 });
            });
        })
    );
    return;
  }

  // ESTRATEGIA 3: Documentos HTML (incluyendo Index.razor compilado)
  if (request.destination === 'document' || request.destination === '') {
    event.respondWith(
      fetch(request)
        .then(response => {
          if (response && response.status === 200) {
            const clonedResponse = response.clone();
            caches.open(CACHE_NAME)
              .then(cache => {
                cache.put(request, clonedResponse);
              });
            return response;
          }
          return response;
        })
        .catch(() => {
          // Intentar servir desde caché (última versión conocida)
          return caches.match(request)
            .then(cached => cached || caches.match('/'))
            .catch(() => new Response('Offline - No hay página en caché', { status: 503 }));
        })
    );
    return;
  }

  // Default: fetch de red
  event.respondWith(fetch(request));
});

// ============================================================
// MENSAJES - Comunicación con el cliente
// ============================================================
self.addEventListener('message', event => {
  console.log('[Service Worker] Message received:', event.data);

  if (event.data && event.data.type === 'SKIP_WAITING') {
    self.skipWaiting();
  }

  if (event.data && event.data.type === 'CLEAR_CACHE') {
    caches.delete(RUNTIME_CACHE).then(() => {
      console.log('[Service Worker] Runtime cache cleared');
      event.ports[0].postMessage({ success: true });
    });
  }

  if (event.data && event.data.type === 'GET_CACHE_SIZE') {
    caches.open(CACHE_NAME).then(cache => {
      cache.keys().then(keys => {
        event.ports[0].postMessage({ cacheSize: keys.length });
      });
    });
  }
});

// ============================================================
// Logs
// ============================================================
console.log('[Service Worker] Ready to serve requests');