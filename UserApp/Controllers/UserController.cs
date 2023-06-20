using Dapper;
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
    public class UserController : Controller
    {

        private readonly UserRepository userRepository;
        private readonly SkillRepository skillRepository;

        public UserController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            userRepository = new UserRepository(connectionString);
            skillRepository = new SkillRepository(connectionString);
        }

        public ActionResult Index()
        {
            var users = userRepository.GetAllUsers();

            foreach (var user in users)
            {
                user.Skills = skillRepository.GetSkillsByUserId(user.UserID);
            }

            return View(users);
        }

        //Create
        public ActionResult Create()
        {
            var skills = skillRepository.GetAllSkills();
            ViewBag.Skills = skills;
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user, List<int> SelectedSkills)
        {
            if (ModelState.IsValid)
            {
                int userId = userRepository.InsertUserAndGetID(user);

                if (SelectedSkills != null && SelectedSkills.Any())
                {
                    userRepository.InsertUserSkills(userId, SelectedSkills);
                }

                return RedirectToAction("Index");
            }

            var skills = skillRepository.GetAllSkills();
            ViewBag.Skills = skills;
            return View(user);
        }


        //Edit

        public ActionResult Edit(int id)
        {
            User user = userRepository.GetUserByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var skills = skillRepository.GetAllSkills();
            ViewBag.Skills = skills;
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user, List<int> SelectedSkills)
        {
            if (ModelState.IsValid)
            {

                userRepository.UpdateUser(user);

                userRepository.UpdateUserSkills(user.UserID, SelectedSkills);
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // Delete
        public ActionResult Delete(int id)
        {
            var user = userRepository.GetUserByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }

        //View
        public ActionResult Details(int id)
        {
            User user = userRepository.GetUserByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        //Restore
        public ActionResult Restore(int id)
        {
            userRepository.Restore(id);

            return RedirectToAction("Index");

        }

        public ActionResult RestoreUs()
        {
            var users = userRepository.RestoreUser();

            foreach (var user in users)
            {
                user.Skills = skillRepository.GetSkillsByUserId(user.UserID);
            }

            return View(users);
        }

        public ActionResult Recover(int id)
        {
            
            userRepository.Recover(id);

            return RedirectToAction("Index");


        }

   

    }
}
