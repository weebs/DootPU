let tick = 0
class AudioProcessor extends AudioWorkletProcessor {
    process(inputs, outputs, parameters) {
// https://stackoverflow.com/questions/61070615/how-can-i-import-a-module-into-an-audioworkletprocessor-that-is-changed-elsewher
        const output = outputs[0]
        output.forEach(channel => {

            // emphasis on sawWave function
            var value = Math.sin(44 * tick * (1.0 / 44000.0)) * 0.8 //sawWave(tick) * 0.1;
            for (let i = 0; i < channel.length; i++) {
                channel[i] = value
            }
        });
        tick++
        return true
    }
}
console.log(AudioProcessor.toString())
console.log(AudioProcessor.prototype)
// console.log(new AudioProcessor())
registerProcessor('audio-processor', AudioProcessor)
