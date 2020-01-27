using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Media;

namespace Audio_Engine
{
    public class Audio_Engine
    {
        private readonly int SAMPLE_RATE;
        private readonly short BIT_DEPTH;
        private readonly int NUM_CHANNELS;
        private const float PI = (float)Math.PI;

        private readonly Func<int, float, short, short> SineWave;
        private readonly Func<int, float, short, short> SquareWave;
        //private readonly Func<int, float, short, short> SawtoothWave;
        //private readonly Func<int, float, short, short> Triangle;
        //private readonly Func<int, float, short, short> WhiteNoise;

        public Audio_Engine(int SampleRate, int numChannels, short bitDepth)
        {
            SAMPLE_RATE = SampleRate;
            NUM_CHANNELS = numChannels;
            BIT_DEPTH = bitDepth;

            SineWave = new Func<int, float, short, short>((position, frequency, amplitude) =>
            {
                return Convert.ToInt16(amplitude * Math.Sin(((PI * 2 * frequency) / SAMPLE_RATE) * position));
            });

            SquareWave = new Func<int, float, short, short>((position, frequency, amplitude) =>
            {
                return Convert.ToInt16(amplitude * Math.Sign(Math.Sin(((PI * 2 * frequency) / SAMPLE_RATE) * position)));
            });

            /*
            SawtoothWave = new Func<int, float, short, short>((position, frequency, amplitude) =>
            {
                int SamplePerWavelenght = Convert.ToInt32(SAMPLE_RATE / (frequency / (float)NUM_CHANNELS));
                short ampStep = Convert.ToInt16((amplitude * 2) / SamplePerWavelenght);
                short tempSample = (short)-amplitude;
                int

                return Convert.ToInt16(short.MaxValue * Math.Sin(((PI * 2 * frequency) / SAMPLE_RATE) * position));
            });
            */
        }

        public void Play(float frequency, short amplitude)
        {
            short[] wave = new short[SAMPLE_RATE * NUM_CHANNELS];
            byte[] bynaryWave = new byte[SAMPLE_RATE * NUM_CHANNELS * sizeof(short)];
            if(NUM_CHANNELS == 1)
                for (int i = 0; i < SAMPLE_RATE; i++)
                {
                        wave[i] = SineWave(i, frequency, amplitude);
                }
            else
                for (int i = 0; i < SAMPLE_RATE; i++)
                {
                    for (int j = 0; j < NUM_CHANNELS; j++)
                    {
                        wave[i + j] = SineWave(i, frequency, amplitude);
                    }
                }
            Buffer.BlockCopy(wave, 0, bynaryWave, 0, wave.Length * 2);
            using (MemoryStream memory = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memory))
            {
                short BlockAllign = (short)(NUM_CHANNELS * BIT_DEPTH / 8);
                int ByteRate = SAMPLE_RATE * BlockAllign;
                int SubChunckSize = (SAMPLE_RATE / BIT_DEPTH) * BlockAllign;
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + SubChunckSize);
                writer.Write("WAVEfmt ".ToCharArray());
                writer.Write(16);
                writer.Write((short)1);
                writer.Write((short)NUM_CHANNELS);
                writer.Write(SAMPLE_RATE);
                writer.Write(ByteRate);
                writer.Write(BlockAllign);
                writer.Write(BIT_DEPTH);
                writer.Write("data".ToCharArray());
                writer.Write(SubChunckSize);
                writer.Write(bynaryWave);
                memory.Position = 0;
                new SoundPlayer(memory).Play();
            }
        }
    }
}
