window.quaggaInterop = {
    startScanner: function (dotNetHelper) {
        Quagga.init({
            inputStream: {
                name: "Live",
                type: "LiveStream",
                target: document.querySelector('#interactive'),
                constraints: {
                    facingMode: "environment",
                    // Forzamos a que la cámara use resolución HD si está disponible
                    width: { min: 640, ideal: 1280 },
                    height: { min: 480, ideal: 720 }
                }
            },
            locator: {
                patchSize: "medium", // Tamaño de los parches de búsqueda (medium es ideal)
                halfSample: true     // Reduce el tiempo de procesamiento
            },
            numOfWorkers: 2, // Usa 2 hilos del procesador para pensar más rápido
            locate: true,    // Obliga a Quagga a buscar activamente la caja del código
            decoder: {
                readers: [
                    "ean_reader", 
                    "upc_reader", 
                    "ean_8_reader", 
                    "code_128_reader"
                ]
            }
        }, function (err) {
            if (err) {
                console.error(err);
                return;
            }
            Quagga.start();
        });

        Quagga.onDetected(function (result) {
            var code = result.codeResult.code;
            
            // Evitamos que lea el mismo código 50 veces por segundo
            if(window.lastScannedCode !== code) {
                window.lastScannedCode = code;
                
                // Llamamos a la función de C#
                dotNetHelper.invokeMethodAsync('ProcesarCodigoEscaneado', code);
                
                // Reseteamos la memoria tras 2 segundos
                setTimeout(() => { window.lastScannedCode = ""; }, 2000);
            }
        });
    },
    stopScanner: function () {
        try {
            Quagga.stop();
        } catch (e) {
            console.log("Quagga ya estaba detenido.");
        }
    }
};