
namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Utility
{
    public interface IUtilityDataRepo
    {
        Task<AmountsOfElements> GetAmountsOfElements();
    }
}