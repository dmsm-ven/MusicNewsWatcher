import { useState, useEffect } from "react";
import "./App.css";
import { Configuration, DownloadHistoryApi } from "./api/client.ts";
            
const cfg = new Configuration({ basePath: "http://localhost:8050" });

function getCookie(name) {
  if (typeof document === "undefined") return "";
  return document.cookie.split("; ").reduce((r, v) => {
    const parts = v.split("=");
    return parts[0] === name ? decodeURIComponent(parts.slice(1).join("=")) : r;
  }, "");
}

function setCookie(name, value, days = 365) {
  if (typeof document === "undefined") return;
  const expires = new Date(Date.now() + days * 864e5).toUTCString();
  document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/`;
}

function App() {
  const [apiResponseJson, setApiResponseJson] = useState("Необходима авторизация");
  const [token, setToken] = useState(() => getCookie("mnw_token") || "");
  const [inputValue, setInputValue] = useState("");

  useEffect(() => {
    async function fetchData() {
      if (!token) {
        setApiResponseJson("no token provided");
        return;
      }

      // create API instance with Authorization header from cookie
      const cfgWithAuth = new Configuration({ basePath: cfg.basePath, headers: { Authorization: `Bearer ${token}` } });
      const apiWithAuth = new DownloadHistoryApi(cfgWithAuth);

      try {
        const res = await apiWithAuth.apiDownloadHistoryGet({ limit: 50 });
        setApiResponseJson("success: " + JSON.stringify(res));
      } catch (e) {
        setApiResponseJson("error: " + String(e));
      }
    }

    fetchData();
  }, []);

  return (
    <>
      <div id="api_data_insert_here">
        <pre>{apiResponseJson}</pre>
      </div>
          <div style={{ margin: '1rem 0' }}>
              <label>
                  Authorization token:
                  <input
                      type="text"
                      value={inputValue}
                      onChange={(e) => setInputValue(e.target.value)}
                      placeholder="Enter bearer token"
                      style={{ marginLeft: '0.5rem', width: '400px' }}
                  />
              </label>
              <button
                  onClick={async () => {
                      setToken(inputValue);
                      setCookie('mnw_token', inputValue);
                      document.location.reload();
                  }}
                  style={{ marginLeft: '0.5rem' }}
              >
                  Save Token
              </button>
        <button
          onClick={() => {
            setToken('');
            setInputValue('');
            setCookie('mnw_token', '', -1);
            setApiResponseJson('token cleared');
          }}
          style={{ marginLeft: '0.5rem' }}
        >
          Clear Token (Exit)
        </button>
      </div>
    </>
  );
}

export default App;
