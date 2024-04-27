using AutoMapper;

namespace FlightManagerApp.Data.Mapping
{
    public interface ICustomMapping
    {
        void ConfigureMapping(IMapperConfigurationExpression mapper);
    }
}
