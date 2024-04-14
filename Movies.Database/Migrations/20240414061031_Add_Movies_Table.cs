using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movies.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Movies_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(2,1)", precision: 2, scale: 1, nullable: false),
                    Length = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Img = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Genres", "Img", "Key", "Length", "Name", "Rate" },
                values: new object[,]
                {
                    { 1, "A former Special Forces operative turned mercenary is subjected to a rogue experiment that leaves him with accelerated healing powers, adopting the alter ego Deadpool.", "Action,Adventure,Comedy", "deadpool.jpg", "deadpool", "1hr 48mins", "Deadpool", 8.6m },
                    { 2, "A veteran pot dealer creates a fake family as part of his plan to move a huge shipment of weed into the U.S. from Mexico.", "Adventure,Comedy,Crime", "we-are-the-millers.jpg", "we-are-the-millers", "1hr 50mins", "We're the Millers", 7.0m },
                    { 3, "The group NWA emerges from the mean streets of Compton in Los Angeles, California, in the mid-1980s and revolutionizes Hip Hop culture with their music and tales about life in the hood.", "Biography,Drama,History", "straight-outta-compton.jpg", "straight-outta-compton", "2hr 27mins", "Straight Outta Compton", 8.0m },
                    { 4, "Teenagers at a juvenile detention center, under the leadership of their counselor, gain self-esteem by playing football together.", "Crime,Drama,Sport", "gridiron-gang.jpg", "gridiron-gang", "2hr 5mins", "Gridiron Gang", 6.9m },
                    { 5, "In 1970s America, a detective works to bring down the drug empire of Frank Lucas, a heroin kingpin from Manhattan, who is smuggling the drug into the country from the Far East.", "Biography,Crime,Drama", "american-gangster.jpg", "american-gangster", "2hr 37mins", "American Gangster", 7.8m },
                    { 6, "It's 1949 Los Angeles, the city is run by gangsters and a malicious mobster, Mickey Cohen. Determined to end the corruption, John O'Mara assembles a team of cops, ready to take down the ruthless leader and restore peace to the city.", "Action,Crime,Drama", "gangster-squad.jpg", "gangster-squad", "1hr 53mins", "Gangster Squad", 6.8m },
                    { 7, "An FBI agent and an Interpol detective track a team of illusionists who pull off bank heists during their performances and reward their audiences with the money.", "Crime,Mystery,Thriller", "now-you-see-me.jpg", "now-you-see-me", "1hr 55mins", "Now You See Me", 7.3m },
                    { 8, "A new theme park is built on the original site of Jurassic Park. Everything is going well until the park's newest attraction--a genetically modified giant stealth killing machine--escapes containment and goes on a killing spree.", "Action,Adventure,Scifi", "jurassic-world.jpg", "jurassic-world", "2hr 4mins", "Jurassic World", 7.1m },
                    { 9, "Ethan and team take on their most impossible mission yet, eradicating the Syndicate - an International rogue organization as highly skilled as they are, committed to destroying the IMF.", "Action,Adventure,Thriller", "mission-impossible-rogue-nation.jpg", "mission-impossible-rogue-nation", "2hr 11mins", "Mission: Impossible: Rogue Nation", 7.5m },
                    { 10, "In the early 1960s, CIA agent Napoleon Solo and KGB operative Illya Kuryakin participate in a joint mission against a mysterious criminal organization, which is working to proliferate nuclear weapons.", "Action,Adventure,Thriller", "the-man-from-uncle.jpg", "the-man-from-uncle", "1hr 56mins", "The Man from U.N.C.L.E.", 7.3m },
                    { 11, "A cryptic message from Bond's past sends him on a trail to uncover a sinister organization. While M battles political forces to keep the secret service alive, Bond peels back the layers of deceit to reveal the terrible truth behind SPECTRE.", "Action,Adventure,Thriller", "spectre.jpg", "spectre", "2hr 28mins", "Spectre", 6.9m },
                    { 12, "The film tells the story of the identical twin gangsters Reggie and Ronnie Kray, two of the most notorious criminals in British history, and their organised crime empire in the East End of London during the 1960s.", "Biography,Crime,Drama", "legend.jpg", "legend", "2hr 28mins", "Legend", 7.0m },
                    { 13, "Boxer Billy Hope turns to trainer Tick Wills to help him get his life back on track after losing his wife in a tragic accident and his daughter to child protection services.", "Action,Drama,Sport", "southpaw.jpg", "southpaw", "2hr 4mins", "Southpaw", 7.5m },
                    { 14, "During the Cold War, an American lawyer is recruited to defend an arrested Soviet spy in court, and then help the CIA facilitate an exchange of the spy for the Soviet captured American U2 spy plane pilot, Francis Gary Powers.", "Biography,Drama,Thriller", "bridge-of-spies.jpg", "bridge-of-spies", "2hr 22mins", "Bridge of Spies", 7.7m },
                    { 15, "Armed with a super-suit with the astonishing ability to shrink in scale but increase in strength, cat burglar Scott Lang must embrace his inner hero and help his mentor, Dr. Hank Pym, plan and pull off a heist that will save the world.", "Action,Adventure,Scifi", "ant-man.jpg", "ant-man", "1hr 57mins", "Ant-Man", 7.4m },
                    { 16, "Deckard Shaw seeks revenge against Dominic Toretto and his family for his comatose brother.", "Action,Crime,Thriller", "fast-and-furious-7.jpg", "fast-and-furious-7", "2hr 17mins", "Fast & Furious 7", 7.3m },
                    { 17, "Wanted by the Chinese mafia, a New York City bike messenger escapes into the world of parkour after meeting a beautiful stranger.", "Action,Crime,Drama", "tracers.jpg", "tracers", "1hr 34mins", "Tracers", 5.6m },
                    { 18, "A low-ranking thug is entrusted by his crime boss to dispose of a gun that killed corrupt cops, but things get out of control when the gun ends up in wrong hands.", "Action,Crime,Drama", "running-scared.jpg", "running-scared", "2hr 2mins", "Running Scared", 7.4m },
                    { 19, "Three buddies wake up from a bachelor party in Las Vegas, with no memory of the previous night and the bachelor missing. They make their way around the city in order to find their friend before his wedding.", "Comedy", "the-hangover.jpg", "the-hangover", "1hr 40mins", "The Hangover", 7.8m },
                    { 20, "3 high school seniors throw a birthday party to make a name for themselves. As the night progresses, things spiral out of control as word of the party spreads.", "Comedy,Crime", "project-x.jpg", "project-x", "1hr 28mins", "Project X", 6.7m },
                    { 21, "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, the caped crusader must come to terms with one of the greatest psychological tests of his ability to fight injustice.", "Action,Crime,Drama", "the-dark-knight.jpg", "the-dark-knight", "2hr 32mins", "The Dark Knight", 9.0m },
                    { 22, "Every seven years in an unsuspecting town, The Tournament takes place. A battle royale between 30 of the world's deadliest assassins. The last man standing receiving the $10,            000,            000 cash prize and the title of Worlds No 1, which itself carries the legendary million dollar a bullet price tag.", "Action,Thriller", "the-tournament.jpg", "the-tournament", "1hr 35mins", "The Tournament", 6.1m },
                    { 23, "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.", "Action,Scifi", "the-matrix.jpg", "the-matrix", "2hr 16mins", "The Matrix", 8.7m },
                    { 24, "Two hip detectives protect a murder witness while investigating a case of stolen heroin.", "Action,Comedy,Crime", "bad-boys.jpg", "bad-boys", "1hr 59mins", "Bad Boys", 6.8m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
