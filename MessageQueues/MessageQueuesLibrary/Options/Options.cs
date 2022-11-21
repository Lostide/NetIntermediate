namespace MessageQueuesLibrary.Options
{
    public class Options
    {
        public static string Sourcefile = "C:\\DataCapture\\video_2022-11-14_19-37-18.mp4";
        public static string SaveFileFolder = "C:\\DataRecieve\\";
        public static int SizeofEachFile = 1048576; //in bytes
        public static string amqpConnectionURI = "amqp://guest:guest@localhost:49156/";
    }
}