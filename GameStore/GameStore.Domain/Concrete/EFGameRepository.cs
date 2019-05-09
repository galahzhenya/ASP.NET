using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Concrete {
    public class EFGameRepository : IGameRepository {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Game> Games {
            get { return context.Games; }
        }
    }
}