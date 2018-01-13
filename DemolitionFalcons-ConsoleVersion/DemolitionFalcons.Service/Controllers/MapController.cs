using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemolitionFalcons.Service.Controllers
{
    using App.Commands.DataProcessor;
    using App.Maps;
    using Front;
    using Microsoft.AspNetCore.Mvc;

    public class MapController : BaseApiController
    {
        [HttpGet]
        public string Get() => Serializer.ExportFirstMapCoordinates(FirstMapFrontEnd.GetMapPath());
    }
    
}
