using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Repositories
{
    internal class ClientAPIRepository : BaseRepository<ClientAPI>, IClientAPIRepository
    {
        public override void AddOrUpdate(ClientAPI model)
        {
            var repository = new RepositoryFactory().CreateGenericRepository<ClientAPI>();
            Task.Run(async () => {
                var oldValue = await repository.GetFirstAsync(x => x.ID == model.ID);
                if (oldValue != null)
                {
                    await repository.UpdateAsync(oldValue, model);

                    var detailRepo = new RepositoryFactory().CreateGenericRepository<APIParameter>();
                    var oldDetails = await detailRepo.GetListAsync(x => x.ApiID == model.ID);

                    if (oldDetails.Count() > 0)
                    {
                        //delete records from the db
                        await detailRepo.ConcreteDeleteRangeAsync(oldDetails);
                    }
                    
                    foreach (var param in model.Params)
                    {
                        //manually assing values of fk here
                        param.ApiID = model.ID;
                    }

                    //adding details as new records in db
                    await detailRepo.AddRangeAsync(model.Params);
                }
                else
                {
                    model.RecordStatus = BaseEntity.RecordStatusType.Active;
                    await repository.AddAsync(model);
                }
            }).Wait();
        }

        public override IQueryable<ClientAPI> GetAll()
        {
            IQueryable<ClientAPI> result = null;

            var repo = new RepositoryFactory().CreateGenericRepository<ClientAPI>();
            repo.ChildNavigationalProperties.Add("Params");

            Task.Run(async () => {
                result = (await repo.GetAllAsync()).AsQueryable();
            }).Wait();

            return result;
        }

        public void DeleteApi(int id)
        {
            try
            {
                //mark as deleted only
                Task.Run(async () => {
                    var apiRepo = new RepositoryFactory().CreateGenericRepository<ClientAPI>();
                    var api = await apiRepo.GetFirstAsync(x => x.ID == id);
                    if (api != null)
                    {
                        await apiRepo.DeleteAsync(api);
                    }

                }).Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
