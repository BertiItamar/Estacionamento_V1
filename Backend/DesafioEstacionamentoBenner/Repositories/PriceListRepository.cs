using DesafioEstacionamentoBenner.Repositories.Interfaces;
using Infrastructure.DataBase;
using Infrastructure.Entities;

namespace DesafioEstacionamentoBenner.Repositories
{
    public class PriceListRepository : BaseRepository<PriceList>, IPriceListRepository
    {
        public PriceListRepository(Context context) : base(context)
        {
        }
    }
}
