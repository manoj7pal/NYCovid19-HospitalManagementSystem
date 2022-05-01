using System;

namespace CovidDashboard.Models
{
    public class DataModel_cs
    {
        public DateTime as_of_date { get; set; }
        public string facility_pfi { get; set; }
        public string facility_name { get; set; }
        public string doh_region { get; set; }
        public string facility_county { get; set; }
        public string facility_network { get; set; }
        public string ny_forward_region { get; set; }
        public string patients_currently { get; set; }
        public string patients_newly_admitted { get; set; }
        public string patients_positive_after { get; set; }
        public string patients_discharged { get; set; }
        public string patients_currently_in_icu { get; set; }
        public string patients_currently_icu { get; set; }
        public string patients_expired { get; set; }
        public string cumulative_covid_19_discharges { get; set; }
        public string cumulative_covid_19_fatalities { get; set; }
        public string total_staffed_acute_care { get; set; }
        public string total_staffed_acute_care_1 { get; set; }
        public string total_staffed_icu_beds_1 { get; set; }
        public string total_staffed_icu_beds_3 { get; set; }
        public string total_new_admissions_reported { get; set; }
        public string patients_age_lt1 { get; set; }
        public string patients_age_1_4 { get; set; }
        public string patients_age_5_19 { get; set; }
        public string patients_age_20_44 { get; set; }
        public string patients_age_45_54 { get; set; }
        public string patients_age_55_64 { get; set; }
        public string patients_age_65_74 { get; set; }
        public string patients_age_75_84 { get; set; }
        public string patients_age_greater_85 { get; set; }
        public string hospitalized_indicator { get; set; }
    }
}
