using mpPInterface;

namespace mpHandDim
{
    public class Interface : IPluginInterface
    {
        private const string _Name = "mpHandDim";
        private const string _AvailCad = "2014";
        private const string _LName = "Ручные размеры";
        private const string _Description = "Выделение цветом, восстановление или удаление размеров с переопределенным значением";
        private const string _Author = "Пекшев Александр aka Modis";
        private const string _Price = "0";
        public string Name { get { return _Name; } }
        public string AvailCad { get { return _AvailCad; } }
        public string LName { get { return _LName; } }
        public string Description { get { return _Description; } }
        public string Author { get { return _Author; } }
        public string Price { get { return _Price; } }

    }
    public class VersionData
    {
        public const string FuncVersion = "2014";
    }
}
