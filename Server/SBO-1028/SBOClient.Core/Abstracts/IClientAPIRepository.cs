using SBOClient.Core.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Abstracts
{
    public interface IClientAPIRepository : IRepository<ClientAPI>
    {
        void DeleteApi(int id);
    }
}
