using Algenta.Colectica.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Algenta.Colectica.Model.Utility;
using CloserSdk.Utility;
using Algenta.Colectica.Model;
using Algenta.Colectica.Model.Ddi;

namespace CloserSdk.Models
{
    public class Item
    {
        public string Agency { get; set; }
        public int Total { get; set; }
    }

    public class Item2
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Item { get; set; }        
        public int ItemNo { get; set; }
        public int Total { get; set; }
    }

    public class Item4
    {
        public string Agency { get; set; }
        public string CompositeId { get; set; }
        public string Description { get; set; }
        public bool IsDeprecated { get; set; }
        public int Total { get; set; }
        public int Questions { get; set; }
        public int QuestionsDeprecated { get; set; }
        public int QuestionsUndeprecated { get; set; }
        public int Questionnaires { get; set; }
        public int QuestionnairesDeprecated { get; set; }
        public int QuestionnairesUndeprecated { get; set; }
        public int Variables { get; set; }
        public int VariablesDeprecated { get; set; }
        public int VariablesUndeprecated { get; set; }
    }

    public class ItemTypesModel
    {
        public string ItemType { get; set; }
        public List<SelectListItem> ItemTypes { get; set; }
        public List<SearchResult> Results { get; set; }
        public IEnumerable<Item> Summary { get; set; }
        public IEnumerable<Item2> Deprecated { get; set; }
        public List<Item4> Breakdown { get; set; }

        public string DataItem(string item)
        {
            string itemtype = "Not Defined";

            if (item == "a51e85bb-6259-4488-8df2-f08cb43485f8") { itemtype = "Physical Instance"; }
            if (item == "ae8ee886-70a2-4c30-a879-b7d92605ba68") { itemtype = "Archive"; }
            if (item == "7e47c269-bcab-40f7-a778-af7bbc4e3d00") { itemtype = "Category"; };
            if (item == "8a3ba89d-70da-4ba1-871a-b41954175453") { itemtype = "Category Group"; }
            if (item == "1c11de94-a36d-4d80-95dc-950c6f37f624") { itemtype = "Category Set"; }
            if (item == "8b108ef8-b642-4484-9c49-f88e4bf7cf1d") { itemtype = "Code Set"; }
            if (item == "394b9ff3-7248-4ede-b945-9bebdbf56bed") { itemtype = "Code List Group"; }
            if (item == "f53f37b2-6f1b-4af3-b89a-4909d512dfb2") { itemtype = "Code List"; }
            if (item == "394b9ff3-7248-4ede-b945-9bebdbf56bed") { itemtype = "Code List Group"; }
            if (item == "48b7d4b4-72bf-470a-a885-720f89bfbc40") { itemtype = "Concept"; }
            if (item == "d38bfe75-60a9-460a-affa-61d643b5416b") { itemtype = "Concept Group"; }
            if (item == "63c9f58d-1ea3-4239-99cf-e4418ec384c5") { itemtype = "Concept Set"; }
            if (item == "d747d7db-ed2a-4339-a156-127f8786d5ec") { itemtype = "Concept Componant"; }
            if (item == "75f63016-b4f8-45b6-953c-f7ac7364fc25") { itemtype = "Concepteptual Variable"; }
            if (item == "dceb4eb2-e7bb-46f8-804d-d9a86aa5ee9f") { itemtype = "Concepteptual Variable Group"; }
            if (item == "ce0f8af6-db9c-4fb3-a31a-e9523fc53668") { itemtype = "Concepteptual Variable Set"; }
            if (item == "2af9a279-89ee-4d06-b2d2-54563a6946ea") { itemtype = "Conditional"; }
            if (item == "ed3801fe-6798-4ea6-808b-73052cc1c633") { itemtype = "Control Construct Set"; }
            if (item == "c5084916-9936-47a9-a523-93be9fd816d8") { itemtype = "Data Collection"; }
            if (item == "f39ff278-8500-45fe-a850-3906da2d242b") { itemtype = "Data Layout"; }
            if (item == "f196cc07-9c99-4725-ad55-5b34f479cf7d") { itemtype = "Instrument"; }
            if (item == "27540e4f-9a3a-415e-8fb9-83c095dc7bcb") { itemtype = "Instrument Group"; }
            if (item == "28455ff2-d6a9-4aa3-9c9f-f6b22b55b3a8") { itemtype = "Organization Group"; }
            if (item == "be33a54f-ca93-454f-9164-8c41df6212cb") { itemtype = "Organization"; }
            if (item == "EACCE1D3-011D-4980-BD4D-6CCFB9161B43") { itemtype = "Other Material"; }
            if (item == "E24AFC19-5618-4050-84C7-1A464AF88485") { itemtype = "Other Material Group"; }
            if (item == "e27aac79-be4a-47d3-96e3-36da178f3923") { itemtype = "Physical Product"; }
            if (item == "b89d26c3-c9e1-4720-a6e4-3a1d8cdb10ce") { itemtype = "Physical Structure"; }
            if (item == "e99acc19-d127-413d-9cf9-aed786e62055") { itemtype = "Processing Event"; }
            if (item == "a64a0ab6-5bfa-43dd-a3fb-e791f8f28c58") { itemtype = "Processing Event Group"; }
            if (item == "c49e2c65-52fb-4948-a454-f3431b834fe7") { itemtype = "Processing Instruction Group"; }
            if (item == "a1a5c54a-3a7b-4dfb-a5fa-46ed8ef465cc") { itemtype = "Processing Instruction Scheme"; }
            if (item == "a1bb19bd-a24a-4443-8728-a6ad80eb42b8") { itemtype = "Question"; }
            if (item == "a1b8a30e-2f35-4056-8467-40e7ed0e7379") { itemtype = "Question Grid"; }
            if (item == "5cc915a1-23c9-4487-9613-779c62f8c205") { itemtype = "Question Group"; }
            if (item == "f433e43d-29a4-4c25-9610-9dd9819a0519") { itemtype = "Question Activity"; }
            if (item == "0a63fcf6-ffdd-4214-b38c-147d6689d6a1") { itemtype = "Question Set"; }
            if (item == "1044459c-8ae2-474a-ad96-6ec18b04953c") { itemtype = "Represented Variable"; }
            if (item == "a8cecef5-4493-47b8-9c83-82a2f1cfb08e") { itemtype = "Represented Variable Group"; }
            if (item == "14404696-2db3-45ac-a94d-139521de6e21") { itemtype = "Represented Variable Set"; }
            if (item == "df457731-a75c-47c3-aeb4-7969d55aa049") { itemtype = "Sequence"; }
            if (item == "4a8b1d85-a508-4b4f-8d56-798219f59689") { itemtype = "Statement"; }
            if (item == "30ea0200-7121-4f01-8d21-a931a182b86d") { itemtype = "Study"; }
            if (item == "683889c6-f74b-4d5e-92ed-908c0a42bb2d") { itemtype = "Variable"; }
            if (item == "91da6c62-c2c2-4173-8958-22c518d1d40d") { itemtype = "Variable Group"; }
            if (item == "50907716-b67a-4dcd-8f9f-8a283cb5fee0") { itemtype = "Variable Set"; }
            if (item == "3b438f9f-e039-4eac-a06d-3fa1aedf48bb") { itemtype = "Variable Statistic"; }
            if (itemtype == "Not Defined")
            {
                return itemtype;
            }
            else
            {
                return itemtype;
            }

        }        
    }
}