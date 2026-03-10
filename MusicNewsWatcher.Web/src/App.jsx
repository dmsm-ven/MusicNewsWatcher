import { useState, useEffect } from "react";
import "./App.css";
import ApiDataDisplay from "./components/ApiDataDisplay";
import TokenControls from "./components/TokenControls";
import { getCookie, setCookie, fetchDownloadHistory } from "./services/apiClient";

function App() {
  const [apiResponseJson, setApiResponseJson] = useState("Необходима авторизация");
  const [token, setToken] = useState(() => getCookie("mnw_token") || "");

  useEffect(() => {
    async function fetchData() {
      if (!token) {
        setApiResponseJson("no token provided");
        return;
      }

      try {
        const res = await fetchDownloadHistory(50, token);
        setApiResponseJson("success: " + JSON.stringify(res));
      } catch (e) {
        setApiResponseJson("error: " + String(e));
      }
    }

    fetchData();
  }, [token]);

  const handleSave = (value) => {
    setToken(value);
    setCookie("mnw_token", value);
  };

  const handleClear = () => {
    setToken("");
    setCookie("mnw_token", "", -1);
    setApiResponseJson("token cleared");
  };

  return (
    <>
      <ApiDataDisplay apiResponseJson={apiResponseJson} />
      <TokenControls token={token} onSave={handleSave} onClear={handleClear} />
    </>
  );
}

export default App;
