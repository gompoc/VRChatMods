namespace ActionMenuApi.Api
{
    /// <summary>
    ///     Supported existing vrchat pages that you can add pedals to
    /// </summary>
    public enum ActionMenuPage
    {
        /// <summary>
        ///     The more "advanced" options in the action menu. Can change menu opacity, size, position etc. here
        /// </summary>
        Config,

        /// <summary>
        ///     The page that shows when you open the emojis page of the action menu
        /// </summary>
        Emojis,

        /// <summary>
        ///     The page that shows when you open the expression page of the action menu using an sdk3 avatar
        /// </summary>
        Expression,

        /// <summary>
        ///     The page that shows when you open the expression page of the action menu using an sdk2 avatar. Has emotes 1-8
        /// </summary>
        SDK2Expression,

        /// <summary>
        ///     The default page that shows when you open the action menu
        /// </summary>
        Main,

        /// <summary>
        ///     The menu opacity page. Has 25%, 50%, 75%, 100%
        /// </summary>
        MenuOpacity,

        /// <summary>
        ///     The menu size page. Has Small, Medium, Large
        /// </summary>
        MenuSize, //Not Implemented

        /// <summary>
        ///     The nameplates config page of the action menu
        /// </summary>
        Nameplates,

        /// <summary>
        ///     The nameplates opacity page 0%,20%,40%,60%,80%,100%
        /// </summary>
        NameplatesOpacity,

        /// <summary>
        ///     The nameplates visibility page shown,icons only,hidden
        /// </summary>
        NameplatesVisibilty,

        /// <summary>
        ///     The nameplates size page tiny,small,normal,medium,large
        /// </summary>
        NameplatesSize,

        /// <summary>
        ///     The options page, toggle mic, close menu etc.
        /// </summary>
        Options,
        
        /// <summary>
        ///     Mods page (duh?)
        /// </summary>
        Mods
    }
}