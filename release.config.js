module.exports = {
	branches: ["main", {"name": "dev", "prerelease": true}],
	repositoryUrl: "https://github.com/Vegari-1/JobOfferService",
	plugins: [
		"@semantic-release/commit-analyzer", // analizira komit poruke i odredjuje narednu verziju
		"@semantic-release/release-notes-generator", // generise release notes na osnovu komita
		"@semantic-release/github" // kreira github release
	]
}