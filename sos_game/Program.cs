using System.Text;

using IFN645_SOS;

class SosApp {
    public static void Main() {
       //create the userInterface
        UserInterface userInterface = new UserInterface();
        //loads the specific menus we want for this instance of the application. multiple interface creators could be designed if necessary
        Ass2InterfaceCreator ass2InterfaceCreator = new Ass2InterfaceCreator();
        ass2InterfaceCreator.CreateInterface(userInterface);
        userInterface.LaunchApp();


    }
}
