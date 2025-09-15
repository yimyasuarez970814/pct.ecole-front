using System.ComponentModel;
using System.Reflection;

namespace pct.ecole_front.Services
{
    public enum TiposServicio
    {
        [Description("Gestión de residuos")]
        GestionResiduos = 1,

        [Description("Transporte especializado")]
        TransporteEspecializado = 2,

        [Description("Mano de obra in house")]
        ManoObraInHouse = 3,

        [Description("Soluciones ambientales personalizadas")]
        SolucionesAmbientalesPersonalizadas = 4,

        [Description("Otro")]
        Otro = 5
    }


    public static class EnumExtensions
    {
        public static string GetDescription(TiposServicio value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
