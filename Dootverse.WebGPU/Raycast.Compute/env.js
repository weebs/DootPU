function log(data) { console.log(data) }
function init(width, height, workerCode) {
    const initialCanvas = document.createElement('canvas')//,
    initialCanvas.onclick = e => { console.log(e); }
        // ctx = initialCanvas.getContext('2d')
    initialCanvas.width = width
    initialCanvas.height = height
    // window.canvas = initialCanvas
    // window.ctx = ctx
    // const imgData = ctx.createImageData(canvas.width, canvas.height)
    document.body.appendChild(initialCanvas)
    const canvas = initialCanvas.transferControlToOffscreen()
    document.body.onkeydown = async (e) => {
        await DotNet.invokeMethodAsync('Raycast.Compute', 'KeyboardInput', e.key, e.code, true)
    }
    document.body.onkeyup = async e => {
        await DotNet.invokeMethodAsync('Raycast.Compute', 'KeyboardInput', e.key, e.code, false)
    }
    var blob = new Blob([workerCode], {type: 'application/javascript'});
    var blobUrl = URL.createObjectURL(blob)
    var worker = new Worker(blobUrl);
    debugger;
    worker.postMessage({
        canvas: canvas
    }, [canvas])
    // window.drawImage = function (buffer) {
    //     imgData.data.set(buffer)
    //     ctx.putImageData(imgData, 0, 0)
    // }
}