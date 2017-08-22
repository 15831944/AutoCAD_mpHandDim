using mpPInterface;

namespace mpHandDim
{
    public class Interface : IPluginInterface
    {
        public string Name => "mpHandDim";
        public string AvailCad => "2018";
        public string LName => "Ручные размеры";
        public string Description => "Выделение цветом, восстановление или удаление размеров с переопределенным значением";
        public string Author => "Пекшев Александр aka Modis";
        public string Price => "0";
    }
    public class VersionData
    {
        public const string FuncVersion = "2018";
    }
}
