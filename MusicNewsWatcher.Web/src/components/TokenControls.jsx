import React, { useEffect, useState } from "react";

export default function TokenControls({ token, onSave, onClear }) {
  const [inputValue, setInputValue] = useState("");

  // clear input when token is present
  useEffect(() => {
    if (token) setInputValue("");
  }, [token]);

  return (
    <div className="token-controls">
          {!token && (
              <>
              <div>
                      <label className="token-controls__label" for="user-token">Authorization token:</label>
                      <input
                          className="token-controls__input"
                          id="user-token"
                          type="text"
                          value={inputValue}
                          onChange={(e) => setInputValue(e.target.value)}
                          placeholder="Enter token"
                      />
                  </div>
            <button className="token-controls__btn token-controls__btn--save" onClick={() => onSave(inputValue)}>
              Save Token
            </button>
              </>  
        )}
      {/* Clear button hidden when no token */}
      {token && (
        <button className="token-controls__btn token-controls__btn--clear" onClick={() => onClear()}>
          Clear Token (Exit)
        </button>
      )}
    </div>
  );
}
