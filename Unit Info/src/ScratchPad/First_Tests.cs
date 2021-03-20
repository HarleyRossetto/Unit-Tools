using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScratchPad
{
    public class First_Tests
    {
        static readonly HttpClient httpClient = new HttpClient();

        #region Old_Tests
        public async Task ProcessAllCourses()
        {
            Stopwatch sw = new Stopwatch();
            string url = string.Format("https://macquarie-prod-handbook.factor5-curriculum.com.au/api/content/render/false/query/+contentType:mq2_pcourse%20+mq2_pcourse.implementationYear:{0}/limit/{1}", 2021, 3000);
            sw.Start();
            var courseListJson = await DownloadWebpageContentAsString(url);
            sw.Stop();
            Console.WriteLine("Download took {0} seconds", sw.Elapsed);
            sw.Restart();
            var courses = JsonConvert.DeserializeObject<MacquarieCourseHandbook.MacquarieCourse>(courseListJson);
            sw.Stop();
            Console.WriteLine("Deserialise took {0} seconds for {1} objects", sw.Elapsed, courses.contentlets.Count);
            Console.Read();
        }

        public async Task Scrape_Units(int year, int limit)
        {
            string url = string.Format("https://coursehandbook.mq.edu.au/api/content/render/false/query/+contentType:mq2_psubject%20+mq2_psubject.implementationYear:{0}/limit/{1}", year, limit);
            var subjectListJson = await DownloadWebpageContentAsString(url);
            var fileName = string.Format("units_{0}_limit_{1}_date_{2}.json", year, limit, DateTime.UtcNow.ToLongTimeString().Replace(":", "-"));
            var fullFile = string.Format("Documentation/{0}", fileName);
            Console.WriteLine(fullFile);
            await File.WriteAllTextAsync(@fullFile, subjectListJson);
        }



        public async Task<string> DownloadWebpageContentAsString(string url)
        {
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task ScrapeTest_UnitHandbook()
        {
            string unitUrl = MacquarieCourseHandbook.GetUnitHandbookUrl("ENVS2364", "2021");
            var webpageContentString = await DownloadWebpageContentAsString(unitUrl);

            MacquarieCourseHandbook.MacquarieUnitData unitData = JsonConvert.DeserializeObject<MacquarieCourseHandbook.MacquarieUnitData>(webpageContentString);
            unitData.DecomposeInnerData();

            Console.WriteLine(unitData.contentlets?[0]?.description);

        }

        public async Task ScrapeTest_Course()
        {
            string courseUrl = MacquarieCourseHandbook.GetCourseHandbookUrl("C000105", "2021");
            var courseJson = await DownloadWebpageContentAsString(courseUrl);

            var courseData = JsonConvert.DeserializeObject<MacquarieCourseHandbook.MacquarieCourse>(courseJson);
            courseData.DecomposeInnerData();
        }
        #endregion



        public static class MacquarieCourseHandbook
        {
            public const string courseHandbookBaseUrlFormat = "https://coursehandbook.mq.edu.au/api/content/render/false/query/+contentType:mq2_psubject%20+mq2_psubject.implementationYear:{0}%20+mq2_psubject.code:{1}";

            public const string courseStructureBaseUrlFormat = "https://macquarie-prod-handbook.factor5-curriculum.com.au/api/content/render/false/query/+contentType:mq2_pcourse%20+mq2_pcourse.implementationYear:{0}%20+mq2_pcourse.code:{1}";
            public const string Year_2019 = "2019";
            public const string Year_2020 = "2020";
            public const string Year_2021 = "2021";

            public static string GetUnitHandbookUrl(string unitCode, string year)
            {
                return string.Format(courseHandbookBaseUrlFormat, year, unitCode);
            }

            public static string GetCourseHandbookUrl(string courseCode, string year)
            {
                return string.Format(courseStructureBaseUrlFormat, year, courseCode);
            }

            public class LearningOutcomes
            {
                public string number { get; set; }
                public string description { get; set; }
                public string order { get; set; }
                public string linking_id { get; set; }
                public string lo_cl_id { get; set; }
                public string code { get; set; }
                public AcademicItem academic_item { get; set; }
                public string cl_id { get; set; }

                public class AcademicItem
                {
                    public string key { get; set; }
                    public string value { get; set; }
                    public string cl_id { get; set; }
                }
            }

            public class KeyValueIDTypeRecord
            {
                public string value { get; set; }
                public string cl_id { get; set; }
                public string key { get; set; }
                public string type { get; set; }
            }

            public class LabelledValuePair
            {
                public LabelledValuePair(string label, string value)
                {
                    this.Label = label;
                    this.Value = value;
                }
                public string Label { get; set; }
                public string Value { get; set; }
            }

            public class MacquarieUnitData
            {
                public class UnitData
                {
                    public string hostName { get; set; }
                    public DateTime modDate { get; set; }
                    public UInt16 creditPoints { get; set; }
                    public string code { get; set; }
                    public string data { get; set; }
                    public UnitDataInner Data { get; set; }
                    public string sysId { get; set; }
                    public string description { get; set; }
                    public string studyLevel { get; set; }
                    public string type { get; set; }
                    public string title { get; set; }
                    public string contentTypeLabel { get; set; }
                    public string baseType { get; set; }
                    public string inode { get; set; }
                    public string mode { get; set; }
                    public bool archived { get; set; }
                    public string host { get; set; }
                    public bool working { get; set; }
                    public bool locked { get; set; }
                    public string stInode { get; set; }
                    public string contentType { get; set; }
                    public bool live { get; set; }
                    public string academicOrg { get; set; }
                    public string owner { get; set; }
                    public string identifier { get; set; }
                    public string level { get; set; }
                    public string studyLevelValue { get; set; }
                    public UInt16 languageId { get; set; }
                    public string active { get; set; }
                    public string URL_MAP_FOR_CONTENT { get; set; }
                    public string teachingPeriod { get; set; }
                    public string version { get; set; }
                    public string parentAcademicOrg { get; set; }
                    public string url { get; set; }
                    public string titleImage { get; set; }
                    public string modUserName { get; set; }
                    public string folder { get; set; }
                    public bool hasTitleImage { get; set; }
                    public UInt16 publishedInHandbook { get; set; }
                    public UInt16 sortOrder { get; set; }
                    public string modUser { get; set; }
                    public string location { get; set; }
                    public string levelDisplay { get; set; }
                    public DateTime effectiveDate { get; set; }
                    public string status { get; set; }
                }

                public class UnitDataInner
                {
                    public LabelledValuePair published_in_handbook { get; set; }
                    public string implementationYear { get; set; }
                    public string duration_ft_max { get; set; }
                    public LabelledValuePair grading_schema { get; set; }
                    public LabelledValuePair study_level { get; set; }
                    public string quota_enrolment_requirements { get; set; }
                    public LabelledValuePair status { get; set; }
                    public string duration_pt_max { get; set; }
                    public string duration_pt_std { get; set; }
                    public KeyValueIDTypeRecord parent_id { get; set; }
                    public string start_date { get; set; }
                    public string exclusions { get; set; }
                    public string content_type { get; set; }
                    public KeyValueIDTypeRecord academic_org { get; set; }
                    public string sms_version { get; set; }
                    public KeyValueIDTypeRecord level { get; set; }
                    public string uac_code { get; set; }
                    public string special_requirements { get; set; }
                    public string duration_pt_min { get; set; }
                    public List<LabelledValuePair> special_unit_type { get; set; }
                    public string version { get; set; }
                    public string code { get; set; }
                    public LabelledValuePair version_status { get; set; }
                    public string class_name { get; set; }
                    public string description { get; set; }
                    public string cl_id { get; set; }
                    public KeyValueIDTypeRecord school { get; set; }
                    public string overview { get; set; }
                    public string title { get; set; }
                    public string end_date { get; set; }
                    public string credit_points { get; set; }
                    public LabelledValuePair duration_pt_period { get; set; }
                    public LabelledValuePair sms_status { get; set; }
                    public string learning_materials { get; set; }
                    public bool special_topic { get; set; }
                    public bool d_gov_cohort_year { get; set; }
                    public KeyValueIDTypeRecord asced_broad { get; set; }
                    public bool publish_tuition_fees { get; set; }
                    public LabelledValuePair placement_proportion { get; set; }
                    public List<string> unit_description { get; set; }

                    public List<LearningOutcomes> unit_learning_outcomes { get; set; }
                    public List<NonScheduledLearningActivity> non_scheduled_learning_activities { get; set; }
                    public List<ScheduledLearningActivity> scheduled_learning_activites { get; set; }
                    public List<EnrolmentRule> enrolement_rules { get; set; }
                    public List<Assessment> assessments { get; set; }
                    public List<string> requisites { get; set; }
                    public List<UnitOffering> unit_offering { get; set; }

                    public string academic_item_type { get; set; }
                    public string credit_points_header { get; set; }
                    public string search_title { get; set; }
                    public string unit_offering_text { get; set; }
                    public List<string> inherent_requirements { get; set; }
                    public LabelledValuePair type { get; set; }
                    // public List<string> other_requirements { get; set; }
                    public string subject_search_title { get; set; }
                    public string external_provider { get; set; }
                    // public List<string> links { get; set; }
                }

                public class LearningActivity
                {
                    public string description { get; set; }
                    public LabelledValuePair activity { get; set; }
                    public string cl_id { get; set; }
                    public string offerings { get; set; }
                    public string applies_to_all_offerings { get; set; }
                }
                public class NonScheduledLearningActivity : LearningActivity { }
                public class ScheduledLearningActivity : LearningActivity { }

                public class EnrolmentRule
                {
                    public string description { get; set; }
                    public LabelledValuePair type { get; set; }
                    public string cl_id { get; set; }
                    public UInt16 order { get; set; }
                }

                public class Assessment
                {
                    public string assessment_title { get; set; }
                    public LabelledValuePair type { get; set; }
                    public string weight { get; set; }
                    public string description { get; set; }
                    public string cl_id { get; set; }
                    public string applies_to_all_offerings { get; set; }
                    public string hurdle_task { get; set; }
                    public string offerings { get; set; }
                    public LabelledValuePair individual { get; set; }
                }

                public class UnitOffering
                {
                    public string fees_domestic { get; set; }
                    public string publish { get; set; }
                    public LabelledValuePair location { get; set; }
                    public string name { get; set; }
                    public string clarification_to_appear_in_handbook { get; set; }
                    public string display_name { get; set; }
                    public KeyValueIDTypeRecord teaching_period { get; set; }

                    public KeyValueIDTypeRecord attendance_mode { get; set; }
                    public string quota_number { get; set; }
                    public LabelledValuePair study_level { get; set; }
                    public string self_enrol { get; set; }
                    public KeyValueIDTypeRecord academic_item { get; set; }
                    public string order { get; set; }
                    public string fees_commonwealth { get; set; }
                    public string fees_international { get; set; }
                    public string course_restrictions { get; set; }
                    public string cl_id { get; set; }
                    public string quota_limit { get; set; }

                }

                public List<UnitData> contentlets { get; set; }

                public UnitData Data
                {
                    get
                    {
                        return contentlets?[0];
                    }
                }

                public void DecomposeInnerData()
                {
                    if (contentlets[0]?.data != null)
                    {
                        //string jsonStringLabelReplacedByKey = contentlets[0].data.Replace("label", "key");
                        contentlets[0].Data = JsonConvert.DeserializeObject<UnitDataInner>(contentlets[0].data);
                    }
                }
            }

            public class MacquarieCourse
            {
                public List<CourseData> contentlets { get; set; }

                public CourseData Data
                {
                    get
                    {
                        return contentlets?[0];
                    }
                }

                public void DecomposeInnerData()
                {
                    contentlets[0].Data = JsonConvert.DeserializeObject<CourseDataInner>(contentlets?[0]?.data);
                    contentlets[0].Curriculum_Structure = JsonConvert.DeserializeObject<CurriculumStructure>(contentlets?[0]?.CurriculumStructure);
                }

                public class CourseData
                {
                    public string hostName { get; set; }
                    public DateTime modDate { get; set; }
                    public string code { get; set; }
                    public string data { get; set; }
                    public CourseDataInner Data { get; set; }
                    public string studyLevel { get; set; }
                    public string title { get; set; }
                    public string contentTypeLabel { get; set; }
                    public string inode { get; set; }
                    public string urlYear { get; set; }
                    public bool archived { get; set; }
                    public string CurriculumStructure { get; set; }
                    public CurriculumStructure Curriculum_Structure { get; set; }
                    public string host { get; set; }
                    public bool working { get; set; }
                    public bool locked { get; set; }
                    public string stInode { get; set; }
                    public string contentType { get; set; }
                    public bool live { get; set; }
                    public string owner { get; set; }
                    public string identifier { get; set; }
                    public string studyLevelValue { get; set; }
                    public UInt16 languageId { get; set; }
                    public string URL_MAP_FOR_CONENT { get; set; }
                    public string parentAcademicOrg { get; set; }
                    public string generic { get; set; }
                    public string url { get; set; }
                    public string titleImage { get; set; }
                    public string modeUserName { get; set; }
                    public string implementationYear { get; set; }
                    public string urlMap { get; set; }
                    public string folder { get; set; }
                    public bool hasTitleImage { get; set; }
                    public UInt16 sortOrder { get; set; }
                    public string modUser { get; set; }
                }

                public class CourseDataInner
                {
                    public LabelledValuePair aqf_level { get; set; }
                    public string implementationYear { get; set; }
                    public List<string> accrediting_bodies { get; set; }
                    public LabelledValuePair published_in_handbook { get; set; }
                    public KeyValueIDTypeRecord academic_org { get; set; }
                    public LabelledValuePair status { get; set; }
                    public KeyValueIDTypeRecord school { get; set; }
                    public string credit_points { get; set; }
                    public LabelledValuePair type { get; set; }
                    public string description { get; set; }
                    public string search_title { get; set; }
                    public string cl_id { get; set; }
                    public string code { get; set; }
                    public string title { get; set; }
                    public string version { get; set; }
                    public string content_type { get; set; }
                    public string abbreviated_name_and_major { get; set; }
                    public string version_name { get; set; }
                    public string course_code { get; set; }
                    public string abbreviated_name { get; set; }
                    public string ext_id { get; set; }
                    public KeyValueIDTypeRecord source { get; set; }
                    public bool active { get; set; }
                    public string class_name { get; set; }
                    public DateTime effective_date { get; set; }
                    public string learning_and_teaching_methods { get; set; }
                    public string overview_and_aims_of_the_course { get; set; }
                    public string support_for_learning { get; set; }
                    public string graduate_destinations_and_employability { get; set; }
                    public string fitness_to_practice { get; set; }
                    public string independent_research { get; set; }
                    public string justify_capstone_unit { get; set; }
                    public string how_will_students_meet_clos_in_this_duration { get; set; }
                    public string what_is_the_internal_structure_of_course_majors { get; set; }
                    public string other_double_degree_considerations { get; set; }
                    public string course_standards_and_quality { get; set; }
                    public string exit { get; set; }
                    public bool part_time { get; set; }
                    public string structure { get; set; }
                    public bool no_enrolment { get; set; }
                    public string publication_information { get; set; }
                    public string internship_placement { get; set; }
                    public string cricos_code { get; set; }
                    public string specialisations { get; set; }
                    public LabelledValuePair govt_special_course_type { get; set; }
                    public List<string> entry_list { get; set; }
                    public bool entry_guarantee { get; set; }
                    public bool police_check { get; set; }
                    public string year_12_prerequisites { get; set; }
                    public string last_review_date { get; set; }
                    public string career_opportunities { get; set; }
                    public string fees_description { get; set; }
                    public string location { get; set; }
                    public string overview { get; set; }
                    public KeyValueIDTypeRecord course_value { get; set; }
                    public string alternative_exits { get; set; }
                    public string progression { get; set; }
                    public string english_language { get; set; }
                    public string ib_maths { get; set; }
                    public string requirements { get; set; }
                    public string qualification_requirement { get; set; }
                    public string articulation_arrangement { get; set; }
                    public LabelledValuePair partner_faculty { get; set; }
                    public bool full_time { get; set; }
                    public string qualifications { get; set; }
                    public string double_degrees { get; set; }
                    public bool international_students { get; set; }
                    public string special_admission { get; set; }
                    public string entry { get; set; }
                    public string atar { get; set; }
                    public DateTime course_data_updated { get; set; }
                    public string prior_learning_recognition { get; set; }
                    public string vce_other { get; set; }
                    public string awards_titles { get; set; }
                    public bool on_campus { get; set; }
                    public string research_areas { get; set; }
                    public string accrediation_start_date { get; set; }
                    public string particiption_enrolment { get; set; }
                    public string vce_english { get; set; }
                    public string ib_other { get; set; }
                    public bool online { get; set; }
                    public string progress_to_masters { get; set; }
                    public string additional_info { get; set; }
                    public bool criscos_disclaimer_applicable { get; set; }
                    public string other_description { get; set; }
                    public bool other { get; set; }
                    public bool health_records_and_privacy { get; set; }
                    public bool information_declaration { get; set; }
                    public string ahegs { get; set; }
                    public string part_time_duration { get; set; }
                    public LabelledValuePair criscos_status { get; set; }
                    public string full_time_duration { get; set; }
                    public bool prohibited_employment_declaration { get; set; }
                    public string minimum_entry_requirements { get; set; }
                    public string accrediation_end_date { get; set; }
                    public string accrediation_end { get; set; }
                    public string post_nominals { get; set; }
                    public string ib_english { get; set; }
                    public string credit_arrangements { get; set; }
                    public string outcomes { get; set; }
                    public string maximum_duration { get; set; }
                    public string major_minors { get; set; }
                    public string vce_maths { get; set; }
                    public string degrees_awarded { get; set; }
                    public string non_year_12_entry { get; set; }
                    public bool working_with_children_check { get; set; }
                    public string entry_pathways_and_adjustment_factors_other_details { get; set; }
                    public LabelledValuePair course_duration_in_years { get; set; }
                    public List<string> entry_pathways_and_adjustment_factors { get; set; }
                    public bool does_undergraduate_principle_26_3_apply { get; set; }
                    public bool formal_articulation_pathway_to_higher_award { get; set; }
                    public string application_method_other_details { get; set; }
                    public string ielts_overall_score { get; set; }
                    public string is_this_an_accelerated_course { get; set; }
                    public string how_does_this_course_deliver_a_capstone_experience { get; set; }
                    public string hours_per_week { get; set; }
                    public LabelledValuePair exclusively_an_exit_award { get; set; }
                    public string ielts_listening_score { get; set; }
                    public string admission_to_combined_double { get; set; }
                    public string ielts_speaking_score { get; set; }
                    public string capstone_or_professional_practice { get; set; }
                    public string external_body { get; set; }
                    public string other_provider_name { get; set; }
                    public string provider_name_and_supporting_documentation { get; set; }
                    public string arrangements { get; set; }
                    public string number_of_weeks { get; set; }
                    public List<KeyValueIDTypeRecord> application_method { get; set; }
                    public bool delivery_with_third_party_provider { get; set; }
                    public bool are_there_additional_admission_points { get; set; }
                    public LabelledValuePair volume_of_learning { get; set; }
                    public string award_abbreviation { get; set; }
                    public string admission_requirements { get; set; }
                    public bool any_double_degree_exclusions { get; set; }
                    public string ielts_writing_score { get; set; }
                    public string ahegs_award_text { get; set; }
                    public bool work_based_training_component { get; set; }
                    public string ielts_reading_score { get; set; }
                    public string wam_required_for_progression { get; set; }
                    public string accrediation_text_for_ahegs { get; set; }
                    public LabelledValuePair provider_name { get; set; }
                    public string assessment_regulations { get; set; }
                    public bool accrediated_by_external_body { get; set; }
                    public bool offered_by_an_external_provider { get; set; }
                    public string assessment { get; set; }
                    public List<string> level2_org_unit_data { get; set; }
                    public List<string> related_associated_items { get; set; }
                    public List<Offering> offering { get; set; }
                    public List<string> study_modes { get; set; }
                    public List<string> additional_admission_points { get; set; }
                    public List<string> course_rules { get; set; }
                    public List<CourseNote> course_notes { get; set; }
                    public List<LearningOutcomes> learning_outcomes { get; set; }
                    public List<Fee> fees { get; set; }
                    public List<string> higher_level_courses_that_students_may_exit_from { get; set; }
                    public List<Level1_OrgUnitData> level1_org_unit_data { get; set; }
                    public List<string> articulations { get; set; }
                    public string course_search_title { get; set; }
                    public string availableInDoubles { get; set; }
                    public string availableDoubles { get; set; }
                    public string credit_points_header { get; set; }
                    public string academic_item_type { get; set; }
                    public List<string> inherent_requirements { get; set; }
                    public string availableAOS { get; set; }
                    public string external_provider { get; set; }
                    public List<string> other_requirements { get; set; }
                    public List<string> links { get; set; }
                    public class Offering
                    {
                        public KeyValuePair<string, string> mode { get; set; }
                        public KeyValuePair<string, string> admission_calendar { get; set; }
                        public string clarification_to_appear_in_handbook { get; set; }
                        public string start_date { get; set; }
                        public string end_date { get; set; }
                        public string comments { get; set; }
                        public KeyValuePair<string, string> language_of_instruction;
                        public string ext_id { get; set; }
                        public bool publish { get; set; }
                        public KeyValuePair<string, string> status { get; set; }
                        public bool offered { get; set; }
                        public string linking_id { get; set; }
                        public string cl_id { get; set; }
                        public string display_name { get; set; }
                        public KeyValuePair<string, string> location { get; set; }
                        public string name { get; set; }
                        public string order { get; set; }
                        public List<string> attendance_type { get; set; }
                        public KeyValuePair<string, string> academic_item { get; set; }
                        public string year { get; set; }
                        public bool entry_point { get; set; }
                    }

                    public class CourseNote
                    {
                        public string note { get; set; }
                        public KeyValuePair<string, string> type { get; set; }
                        public string number { get; set; }
                        public string cl_id { get; set; }
                        public string display_value { get; set; }
                    }

                    public class Fee
                    {

                        public string fee_per_credit_point { get; set; }
                        public string fee_note { get; set; }
                        public string other_fee_type { get; set; }
                        public List<string> intakes { get; set; }
                        public bool applies_to_all_intakes { get; set; }
                        public string estimated_annual_fee { get; set; }
                        public KeyValuePair<string, string> fee_type { get; set; }
                        public string cl_id { get; set; }
                    }

                    public class Level1_OrgUnitData
                    {
                        public KeyValuePair<string, string> parent { get; set; }
                        public string url { get; set; }
                        public string name { get; set; }
                        public string cl_id { get; set; }
                    }
                }

                public class CurriculumStructure
                {
                    public KeyValueIDTypeRecord curriculum_structure { get; set; }
                    public string ai_to_cs_cl_id { get; set; }
                    public string pe_remote_id { get; set; }
                    public KeyValueIDTypeRecord cl_id { get; set; }
                    public string name { get; set; }
                    public string credit_points { get; set; }
                    public KeyValueIDTypeRecord parent_id { get; set; }
                    public string effective_date { get; set; }
                    public KeyValueIDTypeRecord source { get; set; }
                    public string parent_table { get; set; }
                    public List<UnitGroupingContainer> container { get; set; }

                    public class AcademicItem
                    {
                        public string abbr_name { get; set; }
                        public string academic_item_version_name { get; set; }
                        public string academic_item_credit_points { get; set; }
                        public string abbreviated_name_and_major { get; set; }
                        public string academic_item_name { get; set; }
                        public string academic_item_code { get; set; }
                        public string academic_item_url { get; set; }
                        public string order { get; set; }
                        public LabelledValuePair academic_item_type { get; set; }
                        public string cl_id { get; set; }
                        public LabelledValuePair vertical_grouping { get; set; }
                        public LabelledValuePair parent_connector { get; set; }
                        public KeyValueIDTypeRecord parent_record { get; set; }
                        public KeyValueIDTypeRecord owning_structure { get; set; }
                        public KeyValueIDTypeRecord child_record { get; set; }
                        public string credit_points { get; set; }
                        public string child_table { get; set; }
                        public string description { get; set; }
                        public string parent_table { get; set; }

                    }

                    public class UnitGroupingContainer
                    {
                        public string title { get; set; }
                        public LabelledValuePair vertical_grouping { get; set; }
                        public LabelledValuePair horizontal_grouping { get; set; }
                        public string preface { get; set; }
                        public string dynamic_query { get; set; }
                        public string child_table { get; set; }
                        public KeyValueIDTypeRecord child_record { get; set; }
                        public string footnote { get; set; }
                        public string description { get; set; }
                        public string credit_points_max { get; set; }
                        public KeyValueIDTypeRecord parent_record { get; set; }
                        public string parent_table { get; set; }
                        public LabelledValuePair parentConnector { get; set; }
                        public List<string> dynamic_relationship { get; set; }
                        public List<UnitGroupingContainer> container { get; set; }
                        public List<AcademicItem> relationship { get; set; }
                    }
                }
            }
        }
    }
}