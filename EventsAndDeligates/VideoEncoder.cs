namespace EventsAndDeligates
{

    public class VideoEncoderEventArgs : EventArgs
    {
        public Video video { get; set; }
    }

    public class VideoEncoder
    {
        public delegate void VideoEncodeEventHandler(object src, VideoEncoderEventArgs arg);
        public event VideoEncodeEventHandler VideoEncoded;
        public void Encode(Video video)
        {
            Console.WriteLine("Encoding Video...");
            Thread.Sleep(1500); 
            OnVideoEncoded(video);

        }

        public virtual void OnVideoEncoded(Video video)
        {

            if(VideoEncoded != null)
            {
                VideoEncoded(this, new VideoEncoderEventArgs() { video = video});
            }
             
        }
    }
}