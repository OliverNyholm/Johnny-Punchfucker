using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Johnny_Punchfucker
{
    class ParticleEnginge
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; } // returnar lokationen för vektorn
        private List<Particle> particles;
        public static List<Texture2D> textures;
        public Color color;
        

        public ParticleEnginge(List<Texture2D> textures, Vector2 location, Color color) // hämtar en textur och position
        {
            EmitterLocation = location;
            ParticleEnginge.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            this.color = color;
        }

        public void Update()
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)]; // skapar ny random textur
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
            (float)(random.NextDouble() * 4 - 2),
            (float)(random.NextDouble() * 4 - 2));
            
            //velocity.Normalize();
            velocity *= (float)(random.NextDouble());
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 4 - 2); // random direktion


          //  Color color = new Color(
          //(float)(150), 0, 0);
            float size = (float)random.NextDouble();
            int ttl = 20;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl); // returnerar
        }

        public void nrOfParticle()
        {
            int total = 200;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }

        }
    }
}
