namespace Culdcept {
    public class CreatureCard : Card {

        public enum Race {
            Humanoid,
            Animal,
            Dragon,
            Plant,
            Undead
        }

        public Element.Type elementType;
        public Race race;
        public int MHP;
        public int ST;
        // 配置限制
        public CreatureAbility ability;
        // public string description;

        public CreatureCard() {
            type = Type.Creature;
        }
    }
}
