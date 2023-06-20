using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserApp.Models;
using UserApp.Repository;

namespace UserApp.Controllers
{
    public class SkillController : Controller
    {
        private readonly SkillRepository skillRepository;

            public SkillController()
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Data Source=DESKTOP-4QTM0CJ;Initial Catalog=Users;User ID=sa;Password=123"].ConnectionString;
                skillRepository = new SkillRepository(connectionString);
            }

            // GET: Skill
            public ActionResult Index()
            {
                var skills = skillRepository.GetAllSkills();
                return View(skills);
            }

            // GET: Skill/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Skill/Create
            [HttpPost]
            public ActionResult Create(Skill skill)
            {
                if (ModelState.IsValid)
                {
                    skillRepository.InsertSkill(skill);
                    return RedirectToAction("Index");
                }

                return View(skill);
            }

            // GET: Skill/Edit/5
            public ActionResult Edit(int id)
            {
                var skill = skillRepository.GetSkillByID(id);
                if (skill == null)
                {
                    return HttpNotFound();
                }

                return View(skill);
            }

            // POST: Skill/Edit/5
            [HttpPost]
            public ActionResult Edit(Skill skill)
            {
                if (ModelState.IsValid)
                {
                    skillRepository.UpdateSkill(skill);
                    return RedirectToAction("Index");
                }

                return View(skill);
            }

            // GET: Skill/Delete/5
            public ActionResult Delete(int id)
            {
                var skill = skillRepository.GetSkillByID(id);
                if (skill == null)
                {
                    return HttpNotFound();
                }

                return View(skill);
            }

            // POST: Skill/Delete/5
            [HttpPost, ActionName("Delete")]
            public ActionResult DeleteConfirmed(int id)
            {
                skillRepository.DeleteSkill(id);
                return RedirectToAction("Index");
            }
        }

    }
