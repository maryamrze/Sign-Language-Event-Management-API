using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using a2.Models;
using a2.Data;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace a2.Data{
    public class A2Repo : IA2Repo{
        private readonly A2DbContext _dbContext;
        public A2Repo(A2DbContext dbContext){
            _dbContext = dbContext;
        }

        public bool ValidLoginUser(string userName, string password){
            User user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (user == null){
                return false;
            } else {
                return true;
            }
        }

        public bool ValidLoginOrganiser(string Name, string password){
            Organizer organiser = _dbContext.Organizers.FirstOrDefault(u => u.Name == Name && u.Password == password);
            if (organiser == null){
                return false;
            } else {
                return true;
            }
        }
        public User GetUser(User user){
            User userExists = _dbContext.Users.FirstOrDefault(e => e.UserName == user.UserName);
            return userExists;
        }

        public User GetUserbyUserName(string username){
            return _dbContext.Users.FirstOrDefault(e=>e.UserName == username);
        }

        public User AddUser(User user){
            EntityEntry<User> e = _dbContext.Users.Add(user);
            User u = e.Entity;
            _dbContext.SaveChanges();
            return u;
        }
        public Event AddEvent(Event eventinput){
            EntityEntry<Event> e = _dbContext.Events.Add(eventinput);
            Event u = e.Entity;
            _dbContext.SaveChanges();
            return u;
        }

        public IEnumerable<User> GetAllUsers(){
            IEnumerable<User> users = _dbContext.Users.ToList();
            return users;
        }

        public Sign GetSign(string ID){
            var existingSign = _dbContext.Signs.FirstOrDefault(s => s.Id == ID);
            return existingSign;
        }

        public Organizer GetOrganizer(string name){
            var organiserExists = _dbContext.Organizers.FirstOrDefault(o => o.Name == name);
            return organiserExists;
        }

        public int GetCount(){
            return _dbContext.Events.Count();
        }

        public Event GetEvent(int ID){
            return _dbContext.Events.FirstOrDefault(e=>e.Id == ID);
        }
    }
}