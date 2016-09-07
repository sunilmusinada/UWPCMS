using CMS_Survey.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CMS_Survey.Models
{
    public partial class ProvidersLu : ViewModelBase
    {
        #region Properties
        private Int64 providerKey;
        public Int64 ProviderKey
        {
            get { return providerKey; }
            set { if (SetProperty(ref providerKey, value)) IsDirty = true; }
        }
        private string region;
        public string Region
        {
            get { return region; }
            set { if (SetProperty(ref region, value)) IsDirty = true; }
        }
        private string state;
        public string State
        {
            get { return state; }
            set { if (SetProperty(ref state, value)) IsDirty = true; }
        }
        private string cCN;
        public string CCN
        {
            get { return cCN; }
            set { if (SetProperty(ref cCN, value)) IsDirty = true; }
        }
        private string pRVDRINTRNLNUM;
        public string PRVDRINTRNLNUM
        {
            get { return pRVDRINTRNLNUM; }
            set { if (SetProperty(ref pRVDRINTRNLNUM, value)) IsDirty = true; }
        }
        private string fACINTRNLID;
        public string FACINTRNLID
        {
            get { return fACINTRNLID; }
            set { if (SetProperty(ref fACINTRNLID, value)) IsDirty = true; }
        }
        private string facilityName;
        public string FacilityName
        {
            get { return facilityName; }
            set { if (SetProperty(ref facilityName, value)) IsDirty = true; }
        }
        private string typeOfOwnership;
        public string TypeOfOwnership
        {
            get { return typeOfOwnership; }
            set { if (SetProperty(ref typeOfOwnership, value)) IsDirty = true; }
        }
        private string streetAddress;
        public string StreetAddress
        {
            get { return streetAddress; }
            set { if (SetProperty(ref streetAddress, value)) IsDirty = true; }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { if (SetProperty(ref city, value)) IsDirty = true; }
        }
        private string zipCode;
        public string ZipCode
        {
            get { return zipCode; }
            set { if (SetProperty(ref zipCode, value)) IsDirty = true; }
        }
        private string county;
        public string County
        {
            get { return county; }
            set { if (SetProperty(ref county, value)) IsDirty = true; }
        }
        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { if (SetProperty(ref phoneNumber, value)) IsDirty = true; }
        }
        private string stateRegion;
        public string StateRegion
        {
            get { return stateRegion; }
            set { if (SetProperty(ref stateRegion, value)) IsDirty = true; }
        }
        private string certifiedBeds;
        public string CertifiedBeds
        {
            get { return certifiedBeds; }
            set { if (SetProperty(ref certifiedBeds, value)) IsDirty = true; }
        }
        private string providerType;
        public string ProviderType
        {
            get { return providerType; }
            set { if (SetProperty(ref providerType, value)) IsDirty = true; }
        }
        private string providerSubtype;
        public string ProviderSubtype
        {
            get { return providerSubtype; }
            set { if (SetProperty(ref providerSubtype, value)) IsDirty = true; }
        }
        private string deemedStatus;
        public string DeemedStatus
        {
            get { return deemedStatus; }
            set { if (SetProperty(ref deemedStatus, value)) IsDirty = true; }
        }
        private string deemingAgency;
        public string DeemingAgency
        {
            get { return deemingAgency; }
            set { if (SetProperty(ref deemingAgency, value)) IsDirty = true; }
        }
        private bool isDirty = false;
        public bool IsDirty
        {
            get { return isDirty; }
            set { SetProperty(ref isDirty, value); }
        }

        #endregion
        internal ProvidersLu()
        { }
        internal ProvidersLu(Int64 providerKey, string region, string state, string cCN, string pRVDRINTRNLNUM, string fACINTRNLID, string facilityName, string typeOfOwnership, string streetAddress, string city, string zipCode, string county, string phoneNumber, string stateRegion, string certifiedBeds, string providerType, string providerSubtype, string deemedStatus, string deemingAgency)
        {
            this.providerKey = providerKey;
            this.region = region;
            this.state = state;
            this.cCN = cCN;
            this.pRVDRINTRNLNUM = pRVDRINTRNLNUM;
            this.fACINTRNLID = fACINTRNLID;
            this.facilityName = facilityName;
            this.typeOfOwnership = typeOfOwnership;
            this.streetAddress = streetAddress;
            this.city = city;
            this.zipCode = zipCode;
            this.county = county;
            this.phoneNumber = phoneNumber;
            this.stateRegion = stateRegion;
            this.certifiedBeds = certifiedBeds;
            this.providerType = providerType;
            this.providerSubtype = providerSubtype;
            this.deemedStatus = deemedStatus;
            this.deemingAgency = deemingAgency;
        }
        internal List<Hospital> GetHospitalsForState(string StateCode)
        {
            providers_lu_table provTable = new providers_lu_table();
            var Hospitals=provTable.GetHospitalsForState(StateCode);
            return Hospitals;
        }
        internal Hospital GetHospitalForProviderKey(int Key)
        {
            providers_lu_table provTable = new Database.providers_lu_table();
            var Hospital = provTable.GetHospitalForProvCode(Key);
            return Hospital;
        }
    }

}
