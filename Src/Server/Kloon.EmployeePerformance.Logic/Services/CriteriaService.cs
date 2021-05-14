using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Services.Base;
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
        ResultModel<List<CriteriaModel>> GetAll(string key);
        ResultModel<CriteriaModel> Get(Guid id);
        ResultModel<CriteriaModel> Add(CriteriaModel model);
        ResultModel<CriteriaModel> Edit(CriteriaModel model);
        ResultModel<bool> Delete(Guid Id);
        ResultModel<bool> ReOrder(List<CriteriaModel> models);
    }
    public class CriteriaService : ICriteriaService
    {
        private readonly IEntityRepository<Criteria> _criteriaRepository;
        private readonly IAuthenLogicService<CriteriaService> _logicService;
        private readonly IEntityRepository<CriteriaType> _criteriaTypeRepository;
        private readonly IUnitOfWork<EmployeePerformanceContext> _dbContext;
        public CriteriaService(IUnitOfWork<EmployeePerformanceContext> dbContext, IAuthenLogicService<CriteriaService> logicService)
        {
            _dbContext = dbContext;
            _criteriaTypeRepository = _dbContext.GetRepository<CriteriaType>();
            _criteriaRepository = _dbContext.GetRepository<Criteria>();
            _logicService = logicService;
        }

        public ResultModel<CriteriaModel> Add(CriteriaModel model)
        {
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(current =>
                {
                    if (model.TypeId == null)
                    {
                        var isExis = _criteriaTypeRepository.Query().Any(x => x.Name.Equals(model.Name) && !x.DeletedDate.HasValue);
                        if (isExis)
                        {
                            return new ErrorModel(ErrorType.DUPLICATED, "Criteria Type Duplicate");
                        }
                    }
                    else
                    {
                        var isExis = _criteriaRepository.Query().Any(x => x.CriteriaTypeId == model.TypeId && x.Name.Equals(model.Name) && !x.DeletedDate.HasValue);
                        if (isExis)
                        {
                            return new ErrorModel(ErrorType.DUPLICATED, "Criteria Duplicate");
                        }
                    }
                    return null;
                })
                .ThenImplement(x =>
                {
                    Guid id = Guid.NewGuid();
                    if (model.TypeId == null)
                    {
                        var maxOrderType = _criteriaTypeRepository.Query().Count() == 0 ? 0 : _criteriaTypeRepository.Query().Max(t => t.OrderNo);
                        var entity = new CriteriaType
                        {
                            Id = id,
                            Name = model.Name,
                            CreatedBy = 1,
                            OrderNo = maxOrderType + 1,
                            CreatedDate = DateTime.UtcNow,
                            Description = model.Description
                        };
                        _criteriaTypeRepository.Add(entity);
                    }
                    else
                    {
                        var maxOrderType = _criteriaRepository.Query().Count() == 0 ? 0 : _criteriaRepository.Query().Max(t => t.OrderNo);
                        var entity = new Criteria
                        {
                            Id = id,
                            Name = model.Name,
                            CreatedBy = 1,
                            OrderNo = maxOrderType + 1,
                            CreatedDate = DateTime.UtcNow,
                            CriteriaTypeId = model.TypeId.Value,
                            Description = model.Description
                        };
                        _criteriaRepository.Add(entity);
                    }
                    _dbContext.Save();
                    model.Id = id;
                    return model;
                });

            return result;
        }

        public ResultModel<bool> Delete(Guid Id)
        {
            var result = _logicService.Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(x => null)
                .ThenImplement(x =>
                {
                    var entity = _criteriaTypeRepository.Query().Where(x => x.Id == Id && !x.DeletedDate.HasValue).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.DeletedDate = DateTime.UtcNow;
                        entity.DeletedBy = x.Id;
                        _criteriaTypeRepository.Edit(entity);
                    }
                    else
                    {
                        var citeria = _criteriaRepository.Query().Where(x => x.Id == Id && !x.DeletedDate.HasValue).FirstOrDefault();
                        if (citeria == null)
                            return false;

                        citeria.DeletedDate = DateTime.UtcNow;
                        citeria.DeletedBy = x.Id;

                        _criteriaRepository.Edit(citeria);
                    }
                    _dbContext.Save();
                    return true;
                });
            return result;
        }

        public ResultModel<CriteriaModel> Edit(CriteriaModel model)
        {

            var result = _logicService.Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(x =>
                {
                    if (model.TypeId == null)
                    {
                        var isExis = _criteriaTypeRepository.Query().Any(x => x.Name.Equals(model.Name) && !x.DeletedDate.HasValue);
                        if (isExis)
                        {
                            return new ErrorModel(ErrorType.NOT_EXIST, "NotFound_CriteriaType");
                        }
                    }
                    else
                    {
                        var isExis = _criteriaRepository.Query().Any(x => x.CriteriaTypeId == model.TypeId && x.Name.Equals(model.Name) && !x.DeletedDate.HasValue);
                        if (isExis)
                        {
                            return new ErrorModel(ErrorType.NOT_EXIST, "NotFound_Criteria");
                        }
                    }
                    return null;
                })
                .ThenImplement(x => {
                    if (model.TypeId == null)
                    {
                        var entity = _criteriaTypeRepository.Query().Where(x => x.Id.Equals(model.Id) && !x.DeletedDate.HasValue).FirstOrDefault();
                        entity.Name = model.Name;
                        entity.Description = model.Description;
                        entity.ModifiedDate = DateTime.UtcNow;
                        _criteriaTypeRepository.Edit(entity);
                    }
                    else
                    {
                        var entity = _criteriaRepository.Query().Where(x => x.Id.Equals(model.Id) && !x.DeletedDate.HasValue).FirstOrDefault();
                        var maxOrderType = _criteriaRepository.Query().Where(x => x.CriteriaTypeId == model.TypeId).Count() > 0 ?
                            _criteriaRepository.Query().Where(x => x.CriteriaTypeId == model.TypeId).Max(t => t.OrderNo) 
                            : 0;

                        //Update Order change criteria type
                        if (entity.CriteriaTypeId != model.TypeId)
                            entity.OrderNo = maxOrderType + 1;

                        entity.Name = model.Name;
                        entity.Description = model.Description;
                        entity.ModifiedDate = DateTime.UtcNow;
                        entity.CriteriaTypeId = model.TypeId.Value;
                        _criteriaRepository.Edit(entity);
                    }
                    _dbContext.Save();
                    return model;
                });
            return result;
            //if (model.TypeId == null)
            //{
            //    var maxOrderType = _criteriaTypeRepository.Query().Max(t => t.OrderNo);
            //    var entity = _criteriaTypeRepository.Query().Where(x => x.Id.Equals(model.Id) && !x.DeletedDate.HasValue).FirstOrDefault();
            //    if (entity == null)
            //    {
            //        throw new Exception("NotFound_CriteriaType");
            //    }
            //    entity.Name = model.Name;
            //    entity.Description = model.Description;
            //    entity.ModifiedDate = DateTime.UtcNow;
            //    _criteriaTypeRepository.Edit(entity);
            //    _dbContext.Save();

            //    return new CriteriaModel
            //    {
            //        Description = entity.Description,
            //        Id = entity.Id,
            //        Name = entity.Name,
            //        TypeId = null,
            //        OrderNo = entity.OrderNo
            //    };
            //}
            //else
            //{
            //    var entity = _criteriaRepository.Query().Where(x => x.Id.Equals(model.Id) && !x.DeletedDate.HasValue).FirstOrDefault();
            //    if (entity == null)
            //    {
            //        throw new Exception("NotFound_CriteriaType");
            //    }

            //    var maxOrderType = _criteriaRepository.Query().Where(x => x.CriteriaTypeId == model.TypeId).Count() > 0 ?
            //        _criteriaRepository.Query().Where(x => x.CriteriaTypeId == model.TypeId).Max(t => t.OrderNo) : 0;

            //    //Update Order change criteria type
            //    if (entity.CriteriaTypeId != model.TypeId)
            //        entity.OrderNo = maxOrderType + 1;

            //    entity.Name = model.Name;
            //    entity.Description = model.Description;
            //    entity.ModifiedDate = DateTime.UtcNow;
            //    entity.CriteriaTypeId = model.TypeId.Value;



            //    _criteriaRepository.Edit(entity);
            //    _dbContext.Save();

            //    return new CriteriaModel
            //    {
            //        Description = entity.Description,
            //        Id = entity.Id,
            //        Name = entity.Name,
            //        TypeId = null,
            //        OrderNo = entity.OrderNo
            //    };
            //}
        }

        public ResultModel<List<CriteriaModel>> GetAll(string key)
        {
            var result = _logicService.Start()
                .ThenAuthorize()
                .ThenValidate(x => null)
                .ThenImplement(x =>
                {
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
                    if (!string.IsNullOrEmpty(key))
                    {
                        data = data.Where(x => x.Name.Contains(key, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    return data;
                });
            return result;
        }

        public ResultModel<CriteriaModel> Get(Guid id)
        {
            var result = _logicService.Start()
                .ThenAuthorize()
                .ThenValidate(x =>
                {
                    if (id == Guid.Empty)
                    {
                        return new ErrorModel(ErrorType.BAD_REQUEST, "Invalid_Id");
                    }
                    return null;
                })
                .ThenImplement(x => {
                    var data = new CriteriaModel();
                    var criteriaType = _criteriaTypeRepository.Query().Where(t => t.Id == id && !t.DeletedDate.HasValue).FirstOrDefault();
                    if (criteriaType == null)
                    {
                        var criteria = _criteriaRepository.Query().Where(t => t.Id == id && !t.DeletedDate.HasValue).FirstOrDefault();
                        if (criteria == null)
                            return null;
                        data.Id = criteria.Id;
                        data.Description = criteria.Description;
                        data.TypeId = criteria.CriteriaTypeId;
                        data.Name = criteria.Name;
                        data.OrderNo= criteria.OrderNo;
                    }

                    data.Id = criteriaType.Id;
                    data.Description = criteriaType.Description;
                    data.TypeId = null;
                    data.Name = criteriaType.Name;
                    data.OrderNo = criteriaType.OrderNo;

                    return data;
                });
            return result;
        }

        public ResultModel<bool> ReOrder(List<CriteriaModel> models)
        {
            int index = 0;
            models.ForEach(x =>
            {
                x.OrderNo = index++;
            });
            var result = _logicService.Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(t => null)
                .ThenImplement(t => {
                    var criteriaType = _criteriaTypeRepository.Query().Where(x => !x.DeletedDate.HasValue).ToList();
                    var criteria = _criteriaRepository.Query().Where(x => !x.DeletedDate.HasValue).ToList();

                    criteriaType.ForEach(x => {
                        var input = models.Where(t => t.Id == x.Id).FirstOrDefault();
                        if (input != null)
                        {
                            x.OrderNo = input.OrderNo;
                        }
                    });

                    criteria.ForEach(x => {
                        var input = models.Where(t => t.Id == x.Id && t.TypeId != null).FirstOrDefault();
                        if (input != null)
                        {
                            x.OrderNo = input.OrderNo;
                            x.CriteriaTypeId = input.TypeId.Value;
                        }
                    });
                    _dbContext.Save();
                    return true;
                });
            return result;
        }
    }
}
