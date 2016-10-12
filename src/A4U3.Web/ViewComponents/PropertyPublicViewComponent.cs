using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4U3.Domain.Interfaces;
using A4U3.Web.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using A4U3.Web.Helpers;

namespace A4U3.Web.ViewComponents
{
    public class PropertyPublicViewComponent : ViewComponent
    {
        private IRepository _rep;

        public PropertyPublicViewComponent(IRepository repo)
        {
            _rep = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int propertyId, int featureToHighlight, int pictureToHighlight)
        {
            PropertyVM pvm = _rep.GetPropertyAndChildrenById(propertyId).ToPropertyVM();

            pvm.FeatureToHighlight = featureToHighlight;
            pvm.PictureToHighlight = pictureToHighlight;

            return View(pvm);
        }
    }
}
