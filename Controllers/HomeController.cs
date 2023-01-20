using Algenta.Colectica.Model;
using Algenta.Colectica.Model.Ddi;
using Algenta.Colectica.Model.Ddi.Utility;
using Algenta.Colectica.Model.Repository;
using Algenta.Colectica.Model.Utility;
using Algenta.Colectica.Repository.Client;
using CloserSdk.Models;
using CloserSdk.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
// using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ColecticaSdkMvc.Utility;
using System.Threading.Tasks;

namespace CloserSdk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Deletion(ItemTypesModel model, string command)
        {
            var client = ToolkitHelper.RestClient();
            model = Session["Model"] as ItemTypesModel;
            if (model == null) { model = new ItemTypesModel(); }
            switch (command)
            {
                case "Delete":
                    // Task.Run(async () => { await Process(model, client); }).Wait();
                    model = DeleteAllItems(model, client);
                    break;                
                default:
                    var deprecated = GetAllRepositoryItems();
                    model.Deprecated = (from t in deprecated
                                        group t by new { t.ItemType }
                                                   into myGroup
                                        select new Item2
                                        {
                                            Text = myGroup.Key.ItemType.ToString(),
                                            // Value = myGroup.Key.AgencyId,
                                            Total = myGroup.Count()
                                        }).ToList();
                    model.Results = deprecated;
                    model.Deprecated = model.Deprecated.OrderBy(x => x.Text).OrderBy(x => x.Value).ToList();
                    break;

            }            
            Session["Model"] = model;
            return View(model);
        }

        public ItemTypesModel DeleteAllItems(ItemTypesModel model, RestClientItem client)
        {
            List<IdentifierTriple> identifiers = new List<IdentifierTriple>();

            DeleteItems request = new DeleteItems();

            foreach (var result in model.Results)
            {
                identifiers.Add(result.CompositeId);
                request.Identifiers.Add(result.CompositeId);
            }
            var delItemsResult = client.client.DeleteItemsAsync(request);
            
            return model;
        }

        public async Task Process(ItemTypesModel model, RestClientItem clientitem)
        {
            List<IdentifierTriple> identifiers = new List<IdentifierTriple>();

            // Set up a transaction.
            DeleteItems request = new DeleteItems();
            var transaction = await clientitem.client.CreateTransactionAsync();
            // request.TransactionIds.Add(transaction.TransactionId);
            int i = 0;
            // model.Results = model.Results.Where(a => a.ItemType == new Guid("f39ff278-8500-45fe-a850-3906da2d242b")).ToList();
            foreach (var result in model.Results)
            {
                identifiers.Add(result.CompositeId);
                request.Identifiers.Add(result.CompositeId);


                i++;
            }

            // Push the transaction.
            var delItemsResult = await clientitem.client.DeleteItemsAsync(request);

            var commitOptions = new RepositoryTransactionCommitOptions();
            commitOptions.TransactionId = transaction.TransactionId;
            commitOptions.TransactionType = RepositoryTransactionType.CommitAsLatest;
            var commitResult = await clientitem.client.CommitTransactionAsync(commitOptions);

        }

        public ActionResult Deprecated()
        {
            ItemTypesModel model = new ItemTypesModel();
            model.ItemTypes = new List<SelectListItem>();
            model.Breakdown = new List<Item4>();
            model.ItemTypes.Add(new SelectListItem() { Text = "Data File", Value = "Data File" });
            model.ItemTypes.Add(new SelectListItem() { Text = "Instrument", Value = "Instrument" });
            return View(model);
        }

        [HttpPost]
        public ActionResult Deprecated(ItemTypesModel model, string command)
        {
            model.ItemType = command;
            switch (command)
            {
                case "Data File":
                    model = ProcessItemType(model);
                    break;
                case "Instrument":
                    model = ProcessItemType(model);
                    break;
                default:
                    model.Results = GetRepositoryItems(model.ItemType, 0).ToList();
                    break;

            }
            return View(model);
        }

        private List<SearchResult> GetAllRepositoryItems()
        {
            List<SearchResult> deprecated = new List<SearchResult>();

            List<SelectListItem> itemtypes = GetItemTypes();
            foreach (var itemtype in itemtypes)
            {
                List<SearchResult> results = new List<SearchResult>();
                switch (itemtype.Value)
                {
                    case "Category":
                        //results = ProcessCategory(itemtype.Value).Where(a => a.IsDeprecated == true).ToList();
                        break;
                    default:
                        results = GetRepositoryItems(itemtype.Value, 0).Where(a => a.IsDeprecated == true).ToList();
                        break;

                }
                if (results.Count != 0)
                {
                    foreach (var result in results)
                    {
                        deprecated.Add(result);
                    }
                }
            }
            return deprecated;
        }

        // save
        private List<SelectListItem> GetItemTypes()
        {
            List<SelectListItem> itemtypes = new List<SelectListItem>();
            itemtypes.Add(new SelectListItem() { Text = "Archive", Value = "Archive" });
            itemtypes.Add(new SelectListItem() { Text = "Category", Value = "Category" });
            itemtypes.Add(new SelectListItem() { Text = "Category Group", Value = "Category Group" });
            itemtypes.Add(new SelectListItem() { Text = "Category Set", Value = "Category Set" });
            itemtypes.Add(new SelectListItem() { Text = "Code List", Value = "Code List" });
            itemtypes.Add(new SelectListItem() { Text = "Code List Group", Value = "Code List Group" });
            itemtypes.Add(new SelectListItem() { Text = "Code List Scheme", Value = "Code List Scheme" });
            itemtypes.Add(new SelectListItem() { Text = "Code Set", Value = "Code Set" });
            itemtypes.Add(new SelectListItem() { Text = "Concept", Value = "Concept" });
            itemtypes.Add(new SelectListItem() { Text = "Concept Group", Value = "Concept Group" });
            itemtypes.Add(new SelectListItem() { Text = "Concept Scheme", Value = "Concept Scheme" });
            itemtypes.Add(new SelectListItem() { Text = "Conditional", Value = "Conditional" });
            itemtypes.Add(new SelectListItem() { Text = "Data Collection", Value = "Data Collection" });
            itemtypes.Add(new SelectListItem() { Text = "Data File", Value = "Data File" });
            itemtypes.Add(new SelectListItem() { Text = "Data Layout", Value = "Data Layout" });
            itemtypes.Add(new SelectListItem() { Text = "Group", Value = "Group" });
            itemtypes.Add(new SelectListItem() { Text = "Instrument", Value = "Instrument" });
            itemtypes.Add(new SelectListItem() { Text = "Interview Instruction", Value = "Interview Instruction" });
            itemtypes.Add(new SelectListItem() { Text = "Interviewer Instruction", Value = "Interviewer Instruction" });
            itemtypes.Add(new SelectListItem() { Text = "Question", Value = "Question" });
            itemtypes.Add(new SelectListItem() { Text = "Question Activity", Value = "Question Activity" });
            itemtypes.Add(new SelectListItem() { Text = "Question Item", Value = "Question Item" });
            itemtypes.Add(new SelectListItem() { Text = "Question Group", Value = "Question Group" });
            itemtypes.Add(new SelectListItem() { Text = "Question Grid", Value = "Question Grid" });
            itemtypes.Add(new SelectListItem() { Text = "Sequence", Value = "Sequence" });
            itemtypes.Add(new SelectListItem() { Text = "Study", Value = "Study" });
            itemtypes.Add(new SelectListItem() { Text = "Statement", Value = "Statement" });
            itemtypes.Add(new SelectListItem() { Text = "Variable", Value = "Variable" });
            itemtypes.Add(new SelectListItem() { Text = "Variable Group", Value = "Variable Group" });
            itemtypes.Add(new SelectListItem() { Text = "Variable Scheme", Value = "Variable Scheme" });
            itemtypes.Add(new SelectListItem() { Text = "Variable Statistic", Value = "Variable Statistic" });
            return itemtypes;
        }

        private Guid DataItem(string item)
        {
            Guid itemtype = new Guid();
            if (item == "Physical Instance") { itemtype = DdiItemType.PhysicalInstance; }
            if (item == "Archive") { itemtype = DdiItemType.Archive; }
            if (item == "Category") { itemtype = DdiItemType.Category; };
            if (item == "Category Group") { itemtype = DdiItemType.CategoryGroup; }
            if (item == "Category Scheme") { itemtype = DdiItemType.CategoryScheme; }
            if (item == "Category Set") { itemtype = new Guid("1C11DE94-A36D-4D80-95DC-950C6F37F624"); }
            if (item == "CodeList") { itemtype = DdiItemType.CodeList; }
            if (item == "Code List Group") { itemtype = DdiItemType.CodeListGroup; }
            if (item == "Code List Scheme") { itemtype = DdiItemType.CodeListScheme; }
            if (item == "Code Set") { itemtype = new Guid("8B108EF8-B642-4484-9C49-F88E4BF7CF1D"); }
            if (item == "Concept") { itemtype = DdiItemType.Concept; }
            if (item == "Concept Group") { itemtype = DdiItemType.ConceptGroup; }
            if (item == "Concept Scheme") { itemtype = DdiItemType.ConceptScheme; }
            if (item == "Conditional") { itemtype = new Guid("2AF9A279-89EE-4D06-B2D2-54563A6946EA"); }
            if (item == "Data Collection") { itemtype = DdiItemType.DataCollection; }
            if (item == "Data File") { itemtype = new Guid("A51E85BB-6259-4488-8DF2-F08CB43485F8"); }
            if (item == "Data Layout") { itemtype = new Guid("F39FF278-8500-45FE-A850-3906DA2D242B"); }
            if (item == "Group") { itemtype = DdiItemType.Group; }
            if (item == "Instrument") { itemtype = DdiItemType.Instrument; }
            if (item == "Instrument Scheme") { itemtype = DdiItemType.InstrumentScheme; }
            if (item == "Instrument Group") { itemtype = DdiItemType.InstrumentGroup; }
            if (item == "Interviewer Instruction") { itemtype = DdiItemType.OrganizationGroup; }
            if (item == "Interview Instruction") { itemtype = DdiItemType.Instruction; }
            if (item == "Interviewer Instruction Scheme") { itemtype = DdiItemType.Organization; }
            if (item == "Logical Product") { itemtype = DdiItemType.OrganizationGroup; }
            if (item == "Organization") { itemtype = DdiItemType.Organization; }
            if (item == "Organization Group") { itemtype = DdiItemType.OrganizationGroup; }
            if (item == "Organization Scheme") { itemtype = DdiItemType.OrganizationScheme; }
            if (item == "Other Material") { itemtype = DdiItemType.OtherMaterial; }
            if (item == "Other Material Group") { itemtype = DdiItemType.OtherMaterialGroup; }
            if (item == "Physical Structure") { itemtype = DdiItemType.PhysicalStructure; }
            if (item == "Processing Event") { itemtype = DdiItemType.ProcessingEvent; }
            if (item == "Processing Event Group") { itemtype = DdiItemType.ProcessingEventGroup; }
            if (item == "Processing Instruction Group") { itemtype = DdiItemType.ProcessingInstructionGroup; }
            if (item == "Processing Instruction Scheme") { itemtype = DdiItemType.ProcessingInstructionScheme; }
            if (item == "Question") { itemtype = DdiItemType.QuestionItem; }
            if (item == "Question Activity") { itemtype = new Guid("F433E43D-29A4-4C25-9610-9DD9819A0519"); }
            if (item == "Question Grid") { itemtype = DdiItemType.QuestionGrid; }
            if (item == "Question Group") { itemtype = DdiItemType.QuestionGroup; }
            if (item == "Quality Standard") { itemtype = DdiItemType.QualityStandard; }
            if (item == "Quality Statement") { itemtype = DdiItemType.QualityStatement; }
            if (item == "Quality Statement Group") { itemtype = DdiItemType.QualityStatementGroup; }
            if (item == "Represented Variable") { itemtype = DdiItemType.RepresentedVariable; }
            if (item == "Represented Variable Group") { itemtype = DdiItemType.RepresentedVariableGroup; }
            if (item == "Represented Variable Scheme") { itemtype = DdiItemType.RepresentedVariableScheme; }
            if (item == "Statement") { itemtype = DdiItemType.StatementItem; }
            if (item == "Study") { itemtype = DdiItemType.StudyUnit; }
            if (item == "Sequence") { itemtype = DdiItemType.Sequence; }
            if (item == "Variable") { itemtype = DdiItemType.Variable; }
            if (item == "Variable Group") { itemtype = DdiItemType.VariableGroup; }
            if (item == "Variable Scheme") { itemtype = DdiItemType.VariableScheme; }
            if (item == "Variable Statistic") { itemtype = DdiItemType.VariableStatistic; }
            return itemtype;

        }

        public List<SearchResult> GetRepositoryItems(string type, int offset)
        {
            List<SearchResult> results = new List<SearchResult>();
            MultilingualString.CurrentCulture = "en-GB";
            List<SearchResult> responses = new List<SearchResult>();
            SearchFacet facet = new SearchFacet();
            facet.ItemTypes.Add(DataItem(type));
            facet.MaxResults = 200000;
            facet.ResultOffset = offset;
            //facet.SearchTerms.Add("6a9420a5-ecc7-4d8d-bfde-4e34f036f9b5");
            // Set the sort order of the results. Options are 
            // Alphabetical, ItemType, MetadataRank, and VersionDate.
            facet.ResultOrdering = SearchResultOrdering.Alphabetical;
            facet.SearchDepricatedItems = true;
            facet.SearchLatestVersion = true;

            // Now that we have a facet, search for the items in the Repository.
            // The client object takes care of making the Web Services calls.
            //var client = ClientHelper.GetClient();
            var client = ToolkitHelper.RestClient();
            SearchResponse response = client.client.Search(facet);
            responses = response.Results.ToList();
            foreach (var responseitem in responses)
            {
                results.Add(responseitem);
            }

            // Create the model object, and add all the search results to 
            // return response.Results.OrderBy(a => a.VersionDate).ToList();
            var test = results.Where(a => a.IsDeprecated == true).Where(a => a.AgencyId == "uk.cls.bcs70").ToList();
            //results = results.Where(a => a.AgencyId == "uk.iser.ukhls").ToList();
            return results;
        }

        public ItemTypesModel ProcessItemType(ItemTypesModel model)
        {
            var client = ToolkitHelper.RestClient();
            switch (model.ItemType)
            {
                case "Category":
                    model.Results = ProcessCategory(model.ItemType);
                    break;
                default:
                    model.Results = GetRepositoryItems(model.ItemType, 0).ToList();
                    break;
            }
            var questionnaires = GetRepositoryItems("Instrument", 0).ToList();
            List<RepositoryItem> questions = new List<RepositoryItem>();
            foreach (var questionnaire in questionnaires)
            {
                var set = client.client.GetLatestSet(questionnaire.CompositeId);
                var repositoryitems = client.client.GetLatestRepositoryItems(set);
                var questionitems = repositoryitems.Where(a => a.ItemType == DdiItemType.QuestionItem).ToList();
                foreach (var questionitem in questionitems)
                {
                    questions.Add(questionitem);
                }

            }

            model.Breakdown = new List<Item4>();
            foreach (var result in model.Results)
            {
                var set = client.client.GetLatestSet(result.CompositeId);
                var repositoryitems = client.client.GetLatestRepositoryItems(set);
                Item4 currentitem = new Item4();
                currentitem.Agency = result.AgencyId;
                currentitem.CompositeId = result.CompositeId.ToString();
                currentitem.Description = result.DisplayLabel;
                currentitem.IsDeprecated = result.IsDeprecated;
                currentitem.Total = set.Count;
                currentitem.Questions = repositoryitems.Where(a => a.ItemType == DdiItemType.QuestionItem).Count();
                currentitem.QuestionsDeprecated = repositoryitems.Where(a => a.ItemType == DdiItemType.QuestionItem).Where(a => a.IsDeprecated == true).Count();
                currentitem.QuestionsUndeprecated = repositoryitems.Where(a => a.ItemType == DdiItemType.QuestionItem).Where(a => a.IsDeprecated == false).Count();
                currentitem.Variables = repositoryitems.Where(a => a.ItemType == DdiItemType.Variable).Count();
                currentitem.VariablesDeprecated = repositoryitems.Where(a => a.ItemType == DdiItemType.Variable).Where(a => a.IsDeprecated == true).Count();
                currentitem.VariablesUndeprecated = repositoryitems.Where(a => a.ItemType == DdiItemType.Variable).Where(a => a.IsDeprecated == false).Count();
                switch (model.ItemType)
                {
                    case "Data File":
                        if (currentitem.VariablesDeprecated != 0)
                        {
                            currentitem = GetQuestionnareItems(currentitem, questions, result.CompositeId, client);
                            model.Breakdown.Add(currentitem);
                        }
                        break;
                    case "Instrument":
                        if (currentitem.QuestionsDeprecated != 0) { model.Breakdown.Add(currentitem); }
                        break;
                }
                //model.Breakdown.Add(currentitem);
            }
            model.Summary = (from summary in model.Results
                             group summary by summary.DisplayLabel into summaryg
                             select new Item { Agency = summaryg.Key, Total = summaryg.Count() }).ToList();
            model.Breakdown = model.Breakdown.OrderBy(a => a.IsDeprecated).ToList();
            return model;
        }

        public Item4 GetQuestionnareItems(Item4 currentitem, List<RepositoryItem> questions, IdentifierTriple questionnaire, RestClientItem client)
        {
            int deprecated = 0;
            int undeprecated = 0;
            int questioncount = 0;

            var set = client.client.GetLatestSet(questionnaire);
            var repositoryitems = client.client.GetLatestRepositoryItems(set);
            var questionitems = repositoryitems.Where(a => a.ItemType == DdiItemType.QuestionItem).ToList();
            foreach (var questionitem in questionitems)
            {
                var items = questions.Where(a => a.CompositeId == questionitem.CompositeId).ToList();
                if (items.Count != 0)
                {
                    questioncount++;
                    if (items.FirstOrDefault().IsDeprecated == true) { deprecated++; }
                    if (items.FirstOrDefault().IsDeprecated == false) { undeprecated++; }
                }
            }

            currentitem.Questionnaires = questioncount;
            currentitem.QuestionnairesDeprecated = deprecated;
            currentitem.QuestionnairesUndeprecated = undeprecated;
            return currentitem;
        }

        public List<SearchResult> ProcessCategory(string type)
        {
            // Extracts the agency and identifier from selected studies to be used to retrieve repository items for equivalences
            List<SearchResult> groups = new List<SearchResult>();
            List<SearchResult> results = new List<SearchResult>();
            int offset = 1;
            while (offset != 0)
            {
                var responses = GetRepositoryItems("Category", offset - 1);
                offset = offset + responses.Count;
                if (results.Count == 387253) { offset = 0; }
                if (responses.Count == 0) { offset = 0; }
                foreach (var responseitem in responses)
                {
                    results.Add(responseitem);
                }
            }

            return results;
        }
    }
}