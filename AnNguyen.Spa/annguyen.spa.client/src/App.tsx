import { useState } from 'react';
import './App.css';

interface SearchResult {
    indexes: number[];
}
interface SearchRequest {
    query: string;
    match: string;
    limit: number
}

function App() {
    const [searchResult, setSearchResult] = useState<SearchResult | undefined>();
    const [searchRequest, setSearchRequest] = useState<SearchRequest>({
        limit: 100,
        query: '',
        match: ''
    });

    const search = () => {
        const { query, match } = searchRequest
        const isQueryInValid = query.trim().length === 0
        if (isQueryInValid) {
            alert("Please set Search")
            return;
        }

        const isMatchInValid = match.trim().length === 0
        if (isMatchInValid) {
            alert("Please set Match URL")
            return;
        }

        fetch('api/search',
            {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                method: "POST",
                body: JSON.stringify(searchRequest)
            }).then((response) => {
                if (response.ok) {
                    response.json().then(data => {
                        setSearchResult(data)
                    })
                }
            })
            .catch(error => {
                alert(error);
            })
    }

    const onClickHandler = () => {
        search();
    }
    return (
        <div className="main-search">
            <label
                htmlFor="txtQuery"
                className="lable required"
            >Search :</label>
            <input type="text"
                id="txtQuery"
                onChange={(e) => {
                    setSearchRequest({
                        ...searchRequest,
                        query: e.target.value
                    })
                }}
            ></input>
            <br></br>
            <label htmlFor="txtUrl" className="lable required" >Match URL :</label>

            <input type="text"
                id="txtUrl"
                onChange={(e) => {
                    setSearchRequest({
                        ...searchRequest,
                        match: e.target.value
                    })
                }}
            ></input>

            <br></br>
            <input
                type="button"
                value="Search"
                onClick={onClickHandler}
            >
            </input>

            <p>===========================================</p>

            {searchResult?.indexes && <ol>
                {searchResult.indexes.map((item, idex) => {
                    return <li key={idex}>{item}</li>
                })}

                {searchResult.indexes.length == 0 && <li>No items found</li>}
            </ol>}
        </div>
    );


}

export default App;