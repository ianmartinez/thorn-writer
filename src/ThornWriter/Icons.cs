using System;
using Eto.Drawing;

namespace ThornWriter
{
    public enum Resolution
    {
        Standard, Retina
    }

    public static class Icons
    {
        const string standardFolder = "ThornWriter.Resources.icons.raster.standard";
        const string retinaFolder = "ThornWriter.Resources.icons.raster.retina";

        public static Icon Get(string name, Resolution resolution = Resolution.Retina)
        {
            var iconFolder = (resolution == Resolution.Retina) ? retinaFolder : standardFolder;
            return Icon.FromResource(String.Format("{0}.{1}.png", iconFolder, name));
        }
    }
}
