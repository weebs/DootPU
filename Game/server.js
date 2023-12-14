const { chromium, devices } = await import('playwright')
const { spawn } = await import('node:child_process')
const fableWatch = spawn('dotnet', ['fable', 'watch'])
const dotnetSignalingServer = spawn('dotnet', ['run', '--project', '../Signaling.Server/Signaling.Server.fsproj'])
const vite = spawn('cmd.exe', ['/c', 'npx', 'vite', '--host'])
fableWatch.stdout.on('data', data => console.log(data))
dotnetSignalingServer.stdout.on('data', data => console.log(data))
vite.stdout.on('data', data => console.log(data))
setTimeout(async () => {
    const serverBrowser = await chromium.launch({headless: true})
    const serverPage = await serverBrowser.newPage()
    serverPage.on('console', msg => {
        console.log("Server: ", msg._event)
    })
    const gameBrowser = await chromium.launch({headless: false})
    const gamePage = await gameBrowser.newPage()
    await serverPage.goto('http://localhost:5173/dedicated_server.html')
    await gamePage.goto('http://localhost:5173/game.html')
}, 4000);