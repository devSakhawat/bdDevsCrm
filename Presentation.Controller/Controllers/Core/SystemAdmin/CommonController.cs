using Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Core.SystemAdmin;

public class CommonController : BaseApiController
{
  public CommonController(IServiceManager serviceManager) : base(serviceManager) {}

}
