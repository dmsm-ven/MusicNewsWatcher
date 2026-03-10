import { useState, useEffect } from "react";
import "./App.css";
import ApiDataDisplay from "./components/ApiDataDisplay";
import TokenControls from "./components/TokenControls";
import { getCookie, setCookie, fetchDownloadHistory } from "./services/apiClient";
function App() {
    const [downloadHistory, setDownloadHistory] = useState(null);
    const [token, setToken] = useState(() => getCookie("mnw_token") || "");

  useEffect(() => {
      async function fetchData() {
      if (!token) {
          setDownloadHistory([]);
        return;
      }

      try {
        const res = await fetchDownloadHistory(50, token);
          setDownloadHistory(res);
      } catch {
          setDownloadHistory([]);
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
  };

  return (
    <>
      <ApiDataDisplay downloadHistory={downloadHistory} />
      <TokenControls token={token} onSave={handleSave} onClear={handleClear} />
    </>
  );
}

export default App;
