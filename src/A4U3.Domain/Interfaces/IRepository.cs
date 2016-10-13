using System;
using System.Collections.Generic;
using A4U3.Domain.Models;

namespace A4U3.Domain.Interfaces
{
    public interface IRepository : IDisposable
    {
        IStaticData StaticData { get; }

        void AddPropertyAndSave(Property prop);
        void AddFeatureAndSave(Feature feat);
        void AddPictureAndSave(Picture pic);
        void AddUserProfileAndSave(UserProfile up);

        void UpdatePropertyAndSave(Property prop);
        void UpdateFeatureAndSave(Feature feat);
        void UpdatePictureAndSave(Picture pic);
        
        void RemovePropertyAndSave(Property prop);
        void RemoveFeatureAndSave(Feature feat);
        void RemovePictureAndSave(Picture pic);

        Feature GetFeatureById(int id);
        Property GetPropertyById(int id);
        Picture GetPictureById(int id);
        UserProfile GetUserProfile(string username);

        Property GetPropertyAndChildrenById(int id);
        ICollection<Property> GetPropertiesAndChildren(bool enabledOnly = false);

        void Log(string url, string userAgent, string userHostAddress, string controller, string action);
        List<Audit> GetAuditsAll();

        List<Audit> GetAuditsFiltered(bool excludeRobots, bool excludeMe, string searchTerm, string order);
        Audit GetAuditById(int id);

    }
}
