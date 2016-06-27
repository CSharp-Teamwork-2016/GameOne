namespace GameOne.Source.Sound
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Media;

    public class AudioManager
    {
        private Song backgroundMusic;
        
        public void PlayBackgroundMusic(ContentManager content)
        {
            this.backgroundMusic = content.Load<Song>("WoT-Battle-2");
            MediaPlayer.Play(this.backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += this.MediaPlayer_MediaStateChanged;
        }

        protected void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(this.backgroundMusic);
        }
    }
}
