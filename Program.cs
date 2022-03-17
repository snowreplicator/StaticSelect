using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;


/*
select lessons.* 
from
  classifiers.classifier cl
inner join classifiers.classifierset cl_set
        on cl.classifierset_id = cl_set.classifierset_id
        inner join multi_d_cases."LessonSource" lessons
                on cl.value::INTEGER = lessons."LessonSource_id"
where
    cl_set.classnamepk like '%LessonSource%'
 and
    cl.deleted = false
order by
  lessons.code desc,
  lessons."Name" asc
*/

namespace ru.zorro.static_select
{
    public class Program
    {
        private static readonly string DB_CONNECTION_STRING = "Server=127.0.0.1;Port=5432;Database=visual_studio;Username=portal;Password=troP4444";

        static void Main(string[] args)
        {
            ApplicationContext applicationContext = new ApplicationContext(DB_CONNECTION_STRING);
            AllDataInLesonSourceTable(applicationContext);
            //AllActiveDataByCustomSql(applicationContext);
            //AllActiveDataByLinQ(applicationContext);
            AllLessonAndClassifier(applicationContext);
            applicationContext.Dispose();
        }

        // https://docs.microsoft.com/ru-ru/dotnet/csharp/linq/perform-inner-joins
        private static void AllLessonAndClassifier(ApplicationContext applicationContext)
        {
            Console.WriteLine("\n all lesson and classifier:\n");
            int classifierSetId = 1;
            bool delete = true;
            var models = from lessonSource in applicationContext.LessonSources
                         join classifier in applicationContext.Classifiers
                         on new
                         {
                             test1 = lessonSource.LessonSourceId.ToString(),
                             test2 = classifierSetId
                         }
                         equals new
                         {
                             test1 = classifier.Value,
                             test2 = classifier.ClassifiersetId
                         }
                         where classifier.Deleted == delete
                         select lessonSource;
                        /*{
                            lessonid = lessonSource.LessonSourceId,
                            classifierId = classifier.ClassifierId,
                            classifier_classifiersetId = classifier.ClassifiersetId,
                            value = classifier.Value
                        };*/
            foreach (object model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
        }


        // https://metanit.com/sharp/efcore/3.3.php
        // https://metanit.com/sharp/efcore/5.3.php
        private static void AllActiveDataByLinQ(ApplicationContext applicationContext)
        {
            Console.WriteLine("\n data in lesson source table by linq:\n");
            /*

            var classifiers = applicationContext.Classifiers
                .Join(applicationContext.Classifiersets,
                classifier => classifier.ClassifiersetId,
                classifierset => classifierset.ClassifiersetId,
                (classifier, classifierset) => new
                {
                    classifiersetId = classifierset.ClassifiersetId,
                    classnamepk = classifierset.Classnamepk,
                    classifierId = classifier.ClassifierId,
                    classifier_classifiersetId = classifier.ClassifiersetId,
                    value = classifier.Value
                });
            foreach (object model in classifiers)
                Console.WriteLine(JsonConvert.SerializeObject(model));


            Console.WriteLine("\n\n");
            var lessons = from lessonSource in applicationContext.LessonSources
                          join classifier in applicationContext.Classifiers
                          //on lessonSource.LessonSourceId equals classifier.ClassifierId
                          on lessonSource.LessonSourceId.ToString() equals classifier.Value
                          select new
                          {
                              lesson_lessonSourceId = lessonSource.LessonSourceId,
                              classifier_classifiersetId = classifier.ClassifiersetId,
                              classifier_classifierId = classifier.ClassifierId,
                              classifier_value = classifier.Value

                          };
            foreach (object model in lessons)
                Console.WriteLine(JsonConvert.SerializeObject(model));

            return;
            */
            var models = applicationContext.LessonSources
                .Join(applicationContext.Classifiers,
                lessonSource => lessonSource.LessonSourceId,
                //classifier => classifier.ClassifierId,
                // classifier => classifier.ClassifiersetId,
                classifier => int.Parse(classifier.Value),
                //classifier => classifier.Value,
                //classifier => classifier.Value,  //int.Parse(classifier.Value),
                (lessonSource, classifier) => new
                {
                    lessonSourceId = lessonSource.LessonSourceId,
                    value = classifier.Value,
                    classifierId = classifier.ClassifierId,
                    classifiersetId = classifier.ClassifiersetId
                });
            foreach (object model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));




            /*
            var users = db.Users.Join(db.Companies, // второй набор
        u => u.CompanyId, // свойство-селектор объекта из первого набора
        c => c.Id, // свойство-селектор объекта из второго набора
        (u, c) => new // результат
        {
            Name = u.Name,
            Company = c.Name,
            Age = u.Age
        });
            */
            /*

            //int i = int.Parse("");
            var models = applicationContext.LessonSources.Include(u => u.Profile).ToList();
            foreach (LessonSource model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */
            // db.Users.Include(u => u.Profile).ToList())

            /*
            var users = db.Users
                    .Include(u => u.Company)  // подгружаем данные по компаниям
                    .ToList();*/
        }


        // выборка неудаленных данных из LessonSource и Classifier custom sql
        // https://docs.microsoft.com/ru-ru/dotnet/framework/data/adonet/cross-table-queries-linq-to-dataset
        // https://metanit.com/sharp/entityframework/4.4.php
        // https://metanit.com/sharp/efcore/6.1.php
        private static void AllActiveDataByCustomSql(ApplicationContext applicationContext)
        {
            Console.WriteLine("\n data in lesson source table by custom sql:\n");
            string query = "select lessons.*                                                  " +
                           "from                                                              " +
                           "  classifiers.classifier cl                                       " +
                           "inner join classifiers.classifierset cl_set                       " +
                           "        on cl.classifierset_id = cl_set.classifierset_id          " +
                           "        inner join multi_d_cases.\"LessonSource\" lessons         " +
                           "                on cl.value::INTEGER = lessons.\"LessonSource_id\"" +
                           "where                                                             " +
                           "    cl_set.classnamepk like '%LessonSource%'                      " +
                           " and                                                              " +
                           "    cl.deleted = false                                            " +
                           "order by                                                          " +
                           "  lessons.code desc,                                              " +
                           "  lessons.\"Name\" asc                                            ";

            var models = applicationContext.LessonSources.FromSqlRaw(query).ToList();
            foreach (LessonSource model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));

        }

        // выборка из таблицы справочника всех записей
        private static void AllDataInLesonSourceTable(ApplicationContext applicationContext)
        {
            Console.WriteLine("\nall data in lesson source table:\n");


            List<LessonSource> models = applicationContext.LessonSources.ToList();
            foreach (LessonSource model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
        }
    }
}