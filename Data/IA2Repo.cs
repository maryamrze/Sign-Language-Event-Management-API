using a2.Models;

namespace a2.Data{
    public interface IA2Repo{
        public IEnumerable<User> GetAllUsers();
        public bool ValidLoginUser(string userName, string password);
        public bool ValidLoginOrganiser(string Name, string password);
        public User GetUser(User user);
        public User GetUserbyUserName(string username);

        public User AddUser(User user);
        public Event AddEvent(Event eventinput);

        public Sign GetSign(string ID);

        public Organizer GetOrganizer(string name);
        public int GetCount();
        public Event GetEvent(int ID);
    }
}