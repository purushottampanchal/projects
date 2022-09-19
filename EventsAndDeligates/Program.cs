namespace EventsAndDeligates
{
    public class Program
    {

        public static void Main(string[] arg)
        {
            var video = new Video() { Title = "MyVideo1"};
            VideoEncoder videoEncoder = new VideoEncoder();

            videoEncoder.VideoEncoded += new MailService().OnVideoEncoded; 
            videoEncoder.VideoEncoded += new TextService().OnVideoEncoded; 

            videoEncoder.Encode(video);

        }
    }

    public class MailService
    {
        public void OnVideoEncoded(object src, VideoEncoderEventArgs eventArgs)
        {
            Console.WriteLine("Sending email...!"+eventArgs.video.Title);
        }
    }

    public class TextService
    {
        public void OnVideoEncoded(object src, VideoEncoderEventArgs eventArgs)
        {
            Console.WriteLine("Sending Text...! "+eventArgs.video.Title);
        }
    }

}