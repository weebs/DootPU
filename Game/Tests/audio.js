class AudioProcessor extends AudioWorkletProcessor {
    process(inputs, outputs, parameters) {
        const output = outputs[0]
        output.forEach(channel => {

            // emphasis on sawWave function
            var value = sawWave(tick) * 0.1;
            for (let i = 0; i < channel.length; i++) {
                channel[i] = value
            }
        });
        tick++
        return true
    }
}
console.log(AudioProcessor)
console.log(new AudioProcessor())
