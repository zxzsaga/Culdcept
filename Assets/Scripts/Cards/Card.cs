namespace Culdcept {
    [System.Serializable]
    public abstract class Card {

        public enum Type {
            Creature,
            Item,
            Spell
        }

        public Type type;
        public int cost;
    }
}
