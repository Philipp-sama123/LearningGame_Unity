mergeInto(LibraryManager.library, {
  ReactSendMessage: function (message) {
    const msg = UTF8ToString(message); // Convert message
    console.log("Message received from Unity:", msg);

    if (window.ReactReceiveMessage) {
      // Only call ReactReceiveMessage if it exists
      window.ReactReceiveMessage(msg);
    } else {
      console.error("ReactReceiveMessage is not defined.");
    }
  }
});
