using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WorldGen
{
    struct Biome
    {
        public abstract Uri TexturePack { get; }
        public abstract float minHeight { get; }
        public abstract float maxHeight { get; }
        public abstract float minHumidityLevel { get; }
        public abstract float maxHumidityLevel { get; }
        public abstract float minTemperatureLevel { get; }
        public abstract float maxTemperatureLevel { get; }
        public abstract float daylyTempChangeRange { get; }
        public abstract float seasonTempChangeRange { get; }
        public abstract float daylyHumChangeRange { get; }
        public abstract float seasonHumChangeRange { get; }
        public abstract float treeFrequency { get; }
        public abstract float riversFrequency { get; }
        public abstract float lakeFrequency { get; }
        public abstract bool canHaveWaterSources { get; }
        public abstract float bushFrequency { get; }
        public const int Size = 250;
        public abstract bool isValid();
    }

    struct Sea : Biome
    {
        public override Uri TexturePack => throw new NotImplementedException();

        public override float minHeight => 0f;

        public override float maxHeight => 0.1f;

        public override float minHumidityLevel => 0f;

        public override float maxHumidityLevel => 100f;

        public override float minTemperatureLevel => 0f;

        public override float maxTemperatureLevel => 30f;

        public override float daylyTempChangeRange => 20f;

        public override float seasonTempChangeRange => 10f;

        public override float daylyHumChangeRange => 50f;

        public override float seasonHumChangeRange => 25f;

        public override float treeFrequency => 0;

        public override float riversFrequency => 0f;

        public override float lakeFrequency => 0f;

        public override bool canHaveWaterSources => false;

        public override float bushFrequency => 0f;
    }

    struct FrozenSea : Sea
    {
        public override float minTemperatureLevel => -10f;
        public override float maxTemperatureLevel => 0f;
        public override Uri TexturePack => throw new NotImplementedException();
    }

    struct Swamp : Biome
    {
        public override Uri TexturePack => throw new NotImplementedException();

        public override float minHeight => 0.08f;

        public override float maxHeight => 0.13f;

        public override float minHumidityLevel => 75f;

        public override float maxHumidityLevel => 100f;

        public override float minTemperatureLevel => 10f;

        public override float maxTemperatureLevel => 30f;

        public override float daylyTempChangeRange => 5f;

        public override float seasonTempChangeRange => 5f;

        public override float daylyHumChangeRange => 10f;

        public override float seasonHumChangeRange => 5f;

        public override float treeFrequency => 45f;

        public override float riversFrequency => 50f;

        public override float lakeFrequency => 33f;

        public override bool canHaveWaterSources => false;

        public override float bushFrequency => 10f;
    }

    struct Sand : Biome
    {

    }

    struct Plain : Biome
    {
        public override Uri TexturePack => throw new NotImplementedException();

        public override float minHeight => 0.2f;

        public override float maxHeight => 0.6f;

        public override float minHumidityLevel => 20f;

        public override float maxHumidityLevel => 60f;

        public override float minTemperatureLevel => 10f;

        public override float maxTemperatureLevel => 20f;

        public override float daylyTempChangeRange => 10f;

        public override float seasonTempChangeRange => 15f;

        public override float daylyHumChangeRange => 15f;

        public override float seasonHumChangeRange => 10f;

        public override float treeFrequency => 10f;

        public override float riversFrequency => 10f;

        public override float lakeFrequency => 10f;

        public override bool canHaveWaterSources => false;

        public override float bushFrequency => 30f;
    }

    struct FrozenPlain : Plain
    {
        public override Uri TexturePack => throw new NotImplementedException();
        public override float maxTemperatureLevel => 10f;

        public override float minTemperatureLevel => 0f;

    }

    struct Savahna : Plain
    {
        public override Uri TexturePack => throw new NotImplementedException();
        public override float maxTemperatureLevel => 30f;

        public override float minTemperatureLevel => 20f;

        public override float minHumidityLevel => 0;

        public override float maxHumidityLevel => 20;

        public override float treeFrequency => 5f;
    }

    struct Hills : Biome
    {
        public override Uri TexturePack => throw new NotImplementedException();
        public override float minHeight => 0.6f;

        public override float maxHeight => 0.7f;
    }

    struct ForestHill : Hills
    {
        public override Uri TexturePack => throw new NotImplementedException();
        public override float treeFrequency => 45f;
        public override float bushFrequency => 35f;
    }

    struct Forest : Plain
    {
        public override Uri TexturePack => throw new NotImplementedException();
        public override float bushFrequency => 35f;
        public override float treeFrequency => 45f;
    }

    struct Mountains : Biome
    {
        public override Uri TexturePack => throw new NotImplementedException();

        public override float minHeight => throw new NotImplementedException();

        public override float maxHeight => throw new NotImplementedException();

        public override float minHumidityLevel => throw new NotImplementedException();

        public override float maxHumidityLevel => throw new NotImplementedException();

        public override float minTemperatureLevel => throw new NotImplementedException();

        public override float maxTemperatureLevel => throw new NotImplementedException();

        public override float daylyTempChangeRange => throw new NotImplementedException();

        public override float seasonTempChangeRange => throw new NotImplementedException();

        public override float daylyHumChangeRange => throw new NotImplementedException();

        public override float seasonHumChangeRange => throw new NotImplementedException();

        public override float treeFrequency => throw new NotImplementedException();

        public override float riversFrequency => throw new NotImplementedException();

        public override float lakeFrequency => throw new NotImplementedException();

        public override bool canHaveWaterSources => throw new NotImplementedException();

        public override float bushFrequency => throw new NotImplementedException();
    }

    struct Peak : Mountains
    {
        public override Uri TexturePack => throw new NotImplementedException();
        public override float minHeight => 0.9f;
        public override float maxHeight => 1f;

    }

    struct SnowyPeak : Peak
    {

    }

    struct SavahnaHill : Hills
    {

    }

    struct SnowyHill : Hills
    {

    }

    struct ForestMountains : Mountains
    {

    }

    public class GetBiome
    {

    }

}
