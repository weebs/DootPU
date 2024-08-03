onmessage = ev => {
    const canvas = ev.data.canvas
    const ctx = canvas.getContext('2d')
    ctx.scale(10, 10)
    const imgData = ctx.createImageData(canvas.width, canvas.height)
    const renderWs = new WebSocket('ws://127.0.0.1:1338/ws')
    renderWs.onmessage = async e => {
        const buffer = await e.data.arrayBuffer()
        const img = new Uint8ClampedArray(buffer)
        imgData.data.set(img)
        ctx.scale(10, 10)
        ctx.putImageData(imgData, 0, 0)
        // let a = ctx.putImageData
        // (imgData, 0, 0)
        // let a = console.log
        // () (1, 2) (3, 4) (console.log) () 
        // let b = 10
        // return b
    }
}