using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Services
{
    public interface ICriteriaService 
    {
        List<CriteriaModel> GetAll();
        CriteriaModel Add(CriteriaModel model);
        CriteriaModel Edit(CriteriaModel model);
        bool Delete(Guid Id);
    }
    public class CriteriaService : ICriteriaService
    {
        private readonly IEntityRepository<Criteria> _criteriaRepository;
        private readonly IEntityRepository<CriteriaType> _criteriaTypeRepository;
        private readonly IUnitOfWork<EmployeePerformanceContext> _dbContext;
        public CriteriaService(IUnitOfWork<EmployeePerformanceContext> dbContext)
        {
            _dbContext = dbContext;
            _criteriaTypeRepository = _dbContext.GetRepository<CriteriaType>();
            _criteriaRepository = _dbContext.GetRepository<Criteria>();
        }

        public CriteriaModel Add(CriteriaModel model)
        {
            if (model.TypeId == null)
            {
                var maxOrderType = _criteriaTypeRepository.Query().Count() == 0 ? 0 : _criteriaTypeRepository.Query().Max(t => t.OrderNo);
                //var maxOrderType = _criteriaTypeRepository.Query().Max(t => t.OrderNo);
                var isExis = _criteriaTypeRepository.Query().Any(x => x.Name.Equals(model.Name) && !x.DeletedDate.HasValue);
                if (isExis)
                {
                    throw new Exception("Duplicate_CriteriaType");
                }
                var entity = new CriteriaType
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    CreatedBy = 1,
                    OrderNo = maxOrderType + 1,
                    CreatedDate = DateTime.UtcNow,
                    Description = model.Description
                };
                _criteriaTypeRepository.Add(entity);
                _dbContext.Save();
                return new CriteriaModel
                {
                    Description = entity.Description,
                    Id = entity.Id,
                    Name = entity.Name,
                    TypeId = null,
                    OrderNo = entity.OrderNo
                };
            }
            else
            {
                var maxOrderType = _criteriaRepository.Query().Count() == 0 ? 0 : _criteriaRepository.Query().Max(t => t.OrderNo);
                var isExis = _criteriaRepository.Query().Any(x => x.Name.Equals(model.Name) && !x.DeletedDate.HasValue);
                if (isExis)
                {
                    throw new Exception("Duplicate_CriteriaType");
                }
                var entity = new Criteria
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    CreatedBy = 1,
                    OrderNo = maxOrderType + 1,
                    CreatedDate = DateTime.UtcNow,
                    CriteriaTypeId = model.TypeId.Value,
                    Description = model.Description

                };
                _criteriaRepository.Add(entity);
                _dbContext.Save();
                return new CriteriaModel
                {
                    Description = entity.Description,
                    Id = entity.Id,
                    Name = entity.Name,
                    TypeId = model.TypeId,
                    OrderNo = entity.OrderNo
                };
            }
        }

        public bool Delete(Guid Id)
        {
            var entity = _criteriaTypeRepository.Query().Where(x => x.Id == Id && !x.DeletedDate.HasValue).FirstOrDefault();
            if (entity != null)
            {
                entity.DeletedDate = DateTime.UtcNow;
                entity.DeletedBy = 1;
                _criteriaTypeRepository.Edit(entity);
            }
            else {
                var citeria = _criteriaRepository.Query().Where(x => x.Id == Id && !x.DeletedDate.HasValue).FirstOrDefault();
                if (citeria == null)
                    throw new Exception("Nofound");
                citeria.DeletedDate = DateTime.UtcNow;
                citeria.DeletedBy = 1;

                _criteriaRepository.Edit(citeria);
            }
            _dbContext.Save();
            return true;
        }

        public CriteriaModel Edit(CriteriaModel model)
        {
            if (model.TypeId == null)
            {
                var maxOrderType = _criteriaTypeRepository.Query().Max(t => t.OrderNo);
                var entity = _criteriaTypeRepository.Query().Where(x => x.Id.Equals(model.Id) && !x.DeletedDate.HasValue).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception("NotFound_CriteriaType");
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ModifiedDate = DateTime.UtcNow;
                _criteriaTypeRepository.Edit(entity);
                _dbContext.Save();

                return new CriteriaModel
                {
                    Description = entity.Description,
                    Id = entity.Id,
                    Name = entity.Name,
                    TypeId = null,
                    OrderNo = entity.OrderNo
                };
            }
            else
            {
                var entity = _criteriaRepository.Query().Where(x => x.Id.Equals(model.Id) && !x.DeletedDate.HasValue).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception("NotFound_CriteriaType");
                }

                var maxOrderType = _criteriaRepository.Query().Where(x => x.CriteriaTypeId == model.TypeId).Count() > 0 ?
                    _criteriaRepository.Query().Where(x => x.CriteriaTypeId == model.TypeId).Max(t => t.OrderNo) : 0;

                //Update Order change criteria type
                if (entity.CriteriaTypeId != model.TypeId)
                    entity.OrderNo = maxOrderType + 1;

                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ModifiedDate = DateTime.UtcNow;
                entity.CriteriaTypeId = model.TypeId.Value;

                

                _criteriaRepository.Edit(entity);
                _dbContext.Save();

                return new CriteriaModel
                {
                    Description = entity.Description,
                    Id = entity.Id,
                    Name = entity.Name,
                    TypeId = null,
                    OrderNo = entity.OrderNo
                };
            }
        }
        public List<CriteriaModel> GetAll()
        {
            var type = _criteriaTypeRepository.Query()
                .Where(x => !x.DeletedDate.HasValue)
                .OrderBy(x => x.OrderNo)
                .Select(x => new CriteriaModel
                {
                    Id = x.Id,
                    TypeId = null,
                    Description = x.Description,
                    OrderNo = x.OrderNo,
                    Name = x.Name
                }).ToList();
            //var cri = _criteriaRepository.Query().Where(x => !x.DeletedDate.HasValue)
            //    .OrderBy(x => x.OrderNo)
            //    .Select(x => new CriteriaModel
            //    {
            //        Id = x.Id,
            //        TypeId = x.CriteriaTypeId,
            //        Description = x.Description,
            //        OrderNo = x.OrderNo,
            //        Name = x.Name
            //    });
            //var data1 = type.Union(cri).ToList();
            var data = _criteriaTypeRepository.Query()
                .Where(x => !x.DeletedDate.HasValue)
                .Select(x => new CriteriaModel
                {
                    Id = x.Id,
                    TypeId = null,
                    Description = x.Description,
                    OrderNo = x.OrderNo,
                    Name = x.Name
                })
                .Union(_criteriaRepository.Query().Where(x => !x.DeletedDate.HasValue)
                .Select(x => new CriteriaModel
                {
                    Id = x.Id,
                    TypeId = x.CriteriaTypeId,
                    Description = x.Description,
                    OrderNo = x.OrderNo,
                    Name = x.Name
                }))
                .OrderBy(x => x.OrderNo)
                .ToList();
            return data;
        }

    }
}
