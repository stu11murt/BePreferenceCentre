﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BePreferenceCentre.DAL;
using BePreferenceCentre.ViewModels;
using Newtonsoft.Json;
using BePreferenceCentre.Helpers;
using System.ComponentModel;

namespace BePreferenceCentre.ViewModels
{
    public class InkeyViewModel
    {
        BePreferencesEntities db = new BePreferencesEntities();
        public InkeyViewModel()
        {
            answer = new InkeyAnswer();
            InkeyAnswers = db.InkeyAnswers.ToList();
            InkeyJsonAnswers = JsonConvert.SerializeObject(db.InkeyAnswers.ToArray());
        }


        public List<InkeyAnswer> InkeyAnswers { get; set; }
        public InkeyAnswer answer { get; set; }
        
        public string InkeyJsonAnswers { get; set; }

    }
}