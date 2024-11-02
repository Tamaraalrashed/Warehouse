using Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly UnitOfWork _unitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
    }

}
