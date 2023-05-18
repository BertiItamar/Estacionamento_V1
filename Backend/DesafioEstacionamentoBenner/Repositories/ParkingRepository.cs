using DesafioEstacionamentoBenner.Repositories.Interfaces;
using Infrastructure.DataBase;
using Infrastructure.Entities;

namespace DesafioEstacionamentoBenner.Repositories
{
    public class ParkingRepository : BaseRepository<Parking>, IParkingRepository
    {
        public ParkingRepository(Context context) : base(context)
        {
        }
    }
}
